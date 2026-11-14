# Pharmacy System — Database Design V1

Status: proposed baseline for review; do not implement until approved.
Source: Pharmacy.erdplus (1).txt, project business rules and subsequent decisions. Chefaa is a functional/UX reference, not a source of internal schema information.

## Scope and conventions
Single pharmacy, single branch, no tenancy, inventory quantities, ERP integration, product ratings, or returns after successful delivery. Arabic and English catalog. COD only; online payment simulation may be added separately. SQL Server, EF Core, ASP.NET Core Identity.

Notation: `!` required, `?` nullable, PK primary key, FK foreign key, UQ unique. Unless specified, Id is `uniqueidentifier` PK; timestamps use `datetime2(3)` UTC; money uses `decimal(18,2)`; percent uses `decimal(5,2)`; flags use `bit`; enums use `tinyint` with CHECK constraints. Names use `nvarchar(200)`, descriptions `nvarchar(max)`, notes `nvarchar(1000)`, URLs/object keys `nvarchar(1000)`. All FKs required unless marked nullable. No cascading deletion of historical records. Index FKs unless covered by another index. Use rowversion on concurrency-sensitive mutable records.

## 1. Identity and authentication
- AspNetUsers: standard IdentityUser<Guid> fields (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount); custom FirstName!, LastName!, AccountStatus!, CreatedAt!, UpdatedAt!, LastLoginAt?. PhoneNumber required and normalized (+country code), UQ. Filtered UQ NormalizedEmail when not null. Internal username generated; login resolves normalized phone or email. Password hashing delegated to Identity.
- AspNetRoles, AspNetUserRoles, AspNetUserClaims, AspNetRoleClaims, AspNetUserLogins, AspNetUserTokens: standard Identity schema. Seed Admin, Pharmacist, Driver, Customer. UQ AspNetUserRoles.UserId enforces at most one role; transactional account creation enforces exactly one. Staff accounts created by admin; public registration always Customer.
- OtpVerifications: Id, UserId FK, PhoneNumber!, CodeDigest!, Purpose!, CreatedAt!, ExpiresAt!, AttemptCount!, ConsumedAt?, InvalidatedAt?. Never store plaintext OTP; use keyed digest and rate limits by account/phone/IP. Index (UserId, Purpose, CreatedAt). 5 minutes, 5 attempts per code, 60-second resend cooldown; invalidate older codes; atomically consume. Phone confirmation required before first order and after phone change. Email optional; confirmation optional, but verify ownership before enabling email-based recovery.
- AccountStatus proposed: Active, Suspended, Deactivated. Disable login and revoke sessions on suspension/deactivation; retain history. Role restrictions enforced in service layer. Never log secrets.

## 2. Addresses and delivery zones
- DeliveryZones: Id, NameAr!, NameEn!, DeliveryFee!, IsActive!, CreatedAt!, UpdatedAt!. UQ normalized name per language if business requires. Geographic polygons not required V1.
- Addresses: Id, UserId FK, DeliveryZoneId FK, AddressLabel!, Governorate!, City!, StreetName!, BuildingNumber!, FloorNumber?, ApartmentNumber?, AdditionalDirections?, Latitude!, Longitude!, IsDefault!, CreatedAt!, UpdatedAt!. Filtered UQ (UserId) WHERE IsDefault=1. Coordinates decimal(9,6), latitude -90..90, longitude -180..180. Customer only.
- DeliveryConfigurations: singleton Id (int PK CHECK Id=1), FreeDeliveryThreshold? (null disables), UpdatedAt!, UpdatedByUserId FK. Zone fee is authoritative; no conflicting standard fee.

## 3. Catalog
- Manufacturers: Id, Name!, IsActive!. UQ Name.
- Brands: Id, Name!, IsActive!. UQ Name.
- Categories: Id, ParentCategoryId? self FK, NameAr!, NameEn!, DescriptionAr?, DescriptionEn?, IsActive!, DisplayOrder!. Prevent cycles and self-parenting; index ParentCategoryId.
- Products: Id, ManufacturerId FK, BrandId? FK, NameAr!, NameEn!, DescriptionAr?, DescriptionEn?, ProductType!, Price!, IsAvailable!, IsActive!, IsPrescriptionRequired!, MaximumOrderQuantity? (int), CreatedAt!, UpdatedAt!. Types Medicine, Supplement, PersonalCare, MedicalSupply. Price >=0; max quantity >0 if supplied; no stock quantity. Index (IsActive, IsAvailable, ProductType); search both names (full-text optional).
- ProductCategories: ProductId FK, CategoryId FK, composite PK; reverse index (CategoryId, ProductId).
- ProductImages: Id, ProductId FK, ObjectKey!, DisplayOrder!, IsPrimary!, CreatedAt!. Filtered UQ ProductId WHERE IsPrimary=1. Object storage; not binary data in DB.

## 4. Shopping
- ShoppingCarts: Id, CustomerId FK UQ, UpdatedAt!.
- CartItems: CartId FK, ProductId FK, Quantity! int >0, AddedAt!, composite PK (CartId, ProductId).
- Wishlists: Id, CustomerId FK UQ.
- WishlistItems: WishlistId FK, ProductId FK, AddedAt!, composite PK.
Guest cart temporary client-side; on login preserve account cart and discard guest cart. Revalidate availability and limits at checkout.

## 5. Orders and prescriptions
- Orders: Id, OrderNumber! (nvarchar(40), UQ), CustomerId FK, DeliveryZoneId FK, CouponId? FK, OrderType!, Status!, UnavailableItemPolicy!, CreatedAt!, UpdatedAt!, ConfirmedAt?, SubTotal!, DirectDiscountAmount!, CouponDiscountAmount!, DeliveryFee!, Total!, PaymentMethod! (COD), ShippingName!, ShippingPhone!, ShippingZoneName!, ShippingGovernorate!, ShippingCity!, ShippingStreetName!, ShippingBuildingNumber!, ShippingFloorNumber?, ShippingApartmentNumber?, ShippingAdditionalDirections?, ShippingLatitude!, ShippingLongitude!, CancellationReason?. Index (CustomerId, CreatedAt), (Status, CreatedAt). Snapshot shipping fields; no dependency on mutable address. Status proposal: PendingReview, Confirmed, ReadyForDelivery, OutForDelivery, Delivered, Cancelled, Returned.
- OrderItems: Id, OrderId FK, ProductId FK, ProductNameAr!, ProductNameEn!, Quantity! int >0, UnitPrice!, DirectDiscountAmount!, CouponDiscountAmount!, LineTotal!, ItemStatus!, RemovalOrReplacementReason?, ReplacedOrderItemId? self FK. Preserve removed items; exclude them from totals. Replacement item points to original; prevent cross-order links. Index OrderId.
- OrderStatusHistory: Id, OrderId FK, PreviousStatus?, NewStatus!, ChangedAt!, ChangedByUserId? FK (null system), Reason?. Index (OrderId, ChangedAt).
- OrderReviews: Id, OrderId FK, PharmacistId FK, Outcome!, Notes?, ReviewedAt!. Operational review, not customer rating. Index (OrderId, ReviewedAt).
- Prescriptions: Id, OrderId FK UQ, UploadedAt!, ReviewStatus!. Optional one prescription aggregate per order, multiple images.
- PrescriptionImages: Id, PrescriptionId FK, ObjectKey!, UploadedAt!. Private object storage, authorized access only.
- PrescriptionReviews: Id, PrescriptionId FK, PharmacistId FK, Outcome!, Notes?, ReviewedAt!. Retain history. Maintain prescription and order review changes atomically.

Rules: normal/prescription orders reviewed by pharmacist before Confirmed (preparation starts). Normal items: edit quantities/remove; replacements permitted only under explicit Replace policy as linked substitutions (no arbitrary additions). Prescription: pharmacist adds catalog products, no manual prices. If empty, cancel. Policies Replace/Remove/CancelAll; no customer approval for replacement. Customer cancellation only before Confirmed; admin cancellation before terminal completion, with reason (proposed interpretation of earlier any-stage rule). Rejected orders cancelled with reason; retain images and reviews. Reprice at confirmation using current catalog and delivery configuration; freeze at confirmation. A concurrent checkout/confirmation must be atomic and idempotent. No product hard deletion.

Pricing: round discounted unit final price to two decimals, multiply quantity, sum active lines. SubTotal = sum undiscounted active lines; DirectDiscountAmount = direct savings; coupon applied after direct discounts; allocate coupon across eligible lines deterministically with rounding remainder; LineTotal = quantity × rounded discounted unit price minus allocated coupon. Total = SubTotal - DirectDiscountAmount - CouponDiscountAmount + DeliveryFee. Free delivery threshold applies after both discounts. All totals nonnegative. Revalidate eligibility and availability during review. Zone mismatch may be manually reviewed before approval.

## 6. Product requests
- ProductRequests: Id, CustomerId FK, RequestedProductName?, Description?, SubmittedAt!, ReviewStatus!, ReviewedByUserId? FK, ReviewNotes?, ReviewedAt?. Require at least a name, description or image (service invariant).
- ProductRequestImages: Id, ProductRequestId FK, ObjectKey!, UploadedAt!. Private access as appropriate.

## 7. Discounts and coupons
- ProductDiscounts: Id, ProductId FK, Name!, DiscountType!, DiscountValue! decimal(18,2), MinimumQuantity! int default 1, StartsAt!, EndsAt!, IsActive!, CreatedAt!. Fixed amount or percentage; value >0; percentage <=100; end > start. Proposed at most one applicable direct discount, choose highest savings if schedules overlap. No prohibited medicine discounts; eligibility requires pharmacy/legal confirmation before enabling.
- Coupons: Id, Code! nvarchar(50), NormalizedCode! UQ, Description?, DiscountType!, DiscountValue!, StartsAt?, EndsAt?, MinimumOrderValue!, MaximumDiscountAmount?, MaximumUses?, MaximumUsesPerCustomer?, IsActive!, CreatedAt!. Positive limits when supplied; same value checks as discounts.
- CouponUsages: Id, CouponId FK, OrderId FK UQ, Status!, AppliedDiscountAmount!, ReservedAt!, ReleasedAt?, RedeemedAt?. Index (CouponId, Status); per-customer uses resolved via Orders.CustomerId. Reserved/Redeemed/Released; atomic reservation to enforce limits under concurrency. Cancelled orders release usage; proposed Returned orders release usage too. One coupon per order; stack with direct discounts. No customer edits after placing order. Revalidate at Confirmed; if no longer eligible remove and recalculate (proposed).

## 8. Delivery and payment
- Deliveries: Id, OrderId FK UQ, Status!, CreatedAt!, CompletedAt?, FailedAt?, FailureReason?. One attempt; final failure maps order to Returned. No reattempt.
- DeliveryAssignments: Id, DeliveryId FK, DriverId FK, AssignedByUserId FK, AssignedAt!, EndedAt?, EndReason?. Filtered UQ DeliveryId WHERE EndedAt IS NULL. Index (DriverId, EndedAt). Retain assignment history; only current driver may act. Driver only sees assigned orders and necessary contact/address/amount.
- Payments: Id, OrderId FK UQ, Method!, Status!, AmountDue!, AmountCollected?, CollectedAt?, CollectedByUserId? FK. COD Pending/Collected/NotCollected. Record collection atomically with delivery; require amount equal to due for successful COD delivery. No real online gateway in V1.

## 9. Administration
- Notifications: Id, UserId FK, OrderId? FK, Type!, TitleAr!, TitleEn!, BodyAr!, BodyEn!, CreatedAt!, ReadAt?. Index (UserId, ReadAt, CreatedAt). In-site notifications only.
- AuditLogs: Id, ActorUserId? FK, EntityType!, EntityId!, Action!, OldValuesJson?, NewValuesJson?, OccurredAt!. JSON CHECK ISJSON when present. Append-only; restrict access, redact credentials/OTPs and medical data. Index (EntityType, EntityId, OccurredAt).
- SocialMedia: Id, Platform!, Url!, IsActive!, DisplayOrder!, CreatedAt!, UpdatedAt!.
- SystemSettings: singleton Id int PK CHECK Id=1, MetaPixelId?, UpdatedAt!, UpdatedByUserId FK. Admin only; do not expose prescription or sensitive personal details to tracking.

## 10. Cardinality summary
User 1:N Addresses/Orders/ProductRequests/Notifications; User 1:0..1 Cart/Wishlist. Manufacturer 1:N Products; Brand 1:N Products (optional brand per product); Category 1:N child Categories; Product M:N Category. Cart/Wishlist M:N Product through items. Order 1:N OrderItems/StatusHistory/Reviews; Order 1:0..1 Prescription/Delivery/Payment/CouponUsage. Prescription 1:N Images/Reviews. ProductRequest 1:N Images. Delivery 1:N Assignments (max one active); Driver 1:N Assignments. Coupon 1:N Uses; Product 1:N direct discount records.

## 11. Reporting and implementation gates
Delivered orders only count as completed sales; Cancelled, Returned, pending separate. Pharmacist metrics based on reviewed orders; driver success metrics based on delivered orders. Preserve financial history. Validate role permissions, transitions, legal discount rules, OTP limits, concurrency on coupon reservations and COD, snapshots, and privacy. Explicit approval required before creating migrations. No database/code has been generated in this design step.

## 12. Decisions proposed without explicit user approval
GUID IDs; enum representation; staff creation policy; account status names; private image storage; optional brand; singleton settings; exact status names; highest-saving overlapping discount; returned coupon release; coupon revalidation at confirmation; no cancellation after terminal completion; COD amount equality; operational review history tables. These are proposed defaults, not confirmed requirements. Verify regulatory restrictions for medicine discounts and prescription handling before release.
