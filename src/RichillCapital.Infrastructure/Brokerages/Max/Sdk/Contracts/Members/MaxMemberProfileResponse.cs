using Newtonsoft.Json;

namespace RichillCapital.Infrastructure.Brokerages.Max.Sdk.Contracts.Members;

public sealed record MaxMemberProfileResponse
{
    [JsonProperty("sn")]
    public required string Uid { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Language { get; init; }

    [JsonProperty("country_code")]
    public required string CountryCode { get; init; }

    [JsonProperty("phone_number")]
    public required string PhoneNumber { get; init; }

    public required string Status { get; init; }

    [JsonProperty("profile_verified")]
    public required bool ProfileVerified { get; init; }

    [JsonProperty("kyc_state")]
    public required string KycState { get; init; }

    [JsonProperty("any_kyc_rejected")]
    public required bool AnyKycRejected { get; init; }

    [JsonProperty("agreement_checked")]
    public required bool AgreementChecked { get; init; }
    public required int Level { get; init; }

    [JsonProperty("vip_level")]
    public required int VipLevel { get; init; }

    [JsonProperty("member_type")]
    public required string MemberType { get; init; }

    [JsonProperty("phone_contact_approved")]
    public required bool PhoneContactApproved { get; init; }

    [JsonProperty("usd_enabled")]
    public required bool UsdEnabled { get; init; }

    [JsonProperty("referral_code")]
    public required string ReferralCode { get; init; }

    [JsonProperty("birthday")]
    public required string Birthday { get; init; }

    public required string Gender { get; init; }
    public required string Nationality { get; init; }

    [JsonProperty("identity_type")]
    public required string IdentityType { get; init; }

    [JsonProperty("identity_number")]
    public required string IdentityNumber { get; init; }

    [JsonProperty("invoice_carrier_id")]
    public required string InvoiceCarrierId { get; init; }

    [JsonProperty("invoice_carrier_type")]
    public required string InvoiceCarrierType { get; init; }

    public required MaxBankResponse Bank { get; init; }

    [JsonProperty("usd_bank")]
    public required MaxUsdBankResponse UsdBank { get; init; }

    // [JsonProperty("two_factor")]
    // public required string TwoFactor { get; init; }

    // [JsonProperty("current_two_factor_type")]
    // public required string CurrentTwoFactorType { get; init; }

    // [JsonProperty("locked_status_of_2fa")]
    // public required string LockedStatusOf2Fa { get; init; }

    [JsonProperty("supplemental_document_type")]
    public required string SupplementalDocumentType { get; init; }

    [JsonProperty("avatar_url")]
    public required string AvatarUrl { get; init; }

    [JsonProperty("avatar_nft_ownership_sn")]
    public required string AvatarNftOwnershipSn { get; init; }

    [JsonProperty("is_maipay_agent")]
    public required bool IsMaiPayAgent { get; init; }

    public required MaxDocumentsResponse Documents { get; init; }
}

public abstract record MaxBankResponseBase
{
    [JsonProperty("bank_code")]
    public required string BankCode { get; init; }

    [JsonProperty("bank_name")]
    public required string BankName { get; init; }

    public required string Branch { get; init; }

    [JsonProperty("bank_branch_name")]
    public required string BankBranchName { get; init; }

    public required string State { get; init; }

    public required string Account { get; init; }

    [JsonProperty("reject_reason")]
    public required string RejectReason { get; init; }
}

public sealed record MaxBankResponse : MaxBankResponseBase
{
    [JsonProperty("bank_branch_active")]
    public required bool BankBranchActive { get; init; }

    [JsonProperty("intra_bank")]
    public required bool IntraBank { get; init; }
}

public sealed record MaxUsdBankResponse : MaxBankResponseBase
{
}

public sealed record MaxDocumentsResponse
{
    [JsonProperty("photo_id_front_state")]
    public required string PhotoIdFrontState { get; init; }

    [JsonProperty("photo_id_front_reject_reason")]
    public required string PhotoIdFrontRejectReason { get; init; }

    [JsonProperty("photo_id_back_state")]
    public required string PhotoIdBackState { get; init; }

    [JsonProperty("photo_id_back_reject_reason")]
    public required string PhotoIdBackRejectReason { get; init; }

    [JsonProperty("cellphone_bill_state")]
    public required string CellphoneBillState { get; init; }

    [JsonProperty("cellphone_bill_reject_reason")]
    public required string CellphoneBillRejectReason { get; init; }

    [JsonProperty("selfie_with_id_state")]
    public required string SelfieWithIdState { get; init; }

    [JsonProperty("selfie_with_id_reject_reason")]
    public required string SelfieWithIdRejectReason { get; init; }
}