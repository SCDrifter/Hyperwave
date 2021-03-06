/* 
 * EVE Swagger Interface
 *
 * An OpenAPI for EVE Online
 *
 * OpenAPI spec version: 1.2.9
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = Eve.Api.Client.SwaggerDateConverter;

namespace Eve.Api.Model
{
    /// <summary>
    /// 200 ok object
    /// </summary>
    [DataContract]
    public partial class GetContractsPublicRegionId200Ok :  IEquatable<GetContractsPublicRegionId200Ok>, IValidatableObject
    {
        /// <summary>
        /// Type of the contract
        /// </summary>
        /// <value>Type of the contract</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum TypeEnum
        {
            
            /// <summary>
            /// Enum Unknown for "unknown"
            /// </summary>
            [EnumMember(Value = "unknown")]
            Unknown,
            
            /// <summary>
            /// Enum Itemexchange for "item_exchange"
            /// </summary>
            [EnumMember(Value = "item_exchange")]
            Itemexchange,
            
            /// <summary>
            /// Enum Auction for "auction"
            /// </summary>
            [EnumMember(Value = "auction")]
            Auction,
            
            /// <summary>
            /// Enum Courier for "courier"
            /// </summary>
            [EnumMember(Value = "courier")]
            Courier,
            
            /// <summary>
            /// Enum Loan for "loan"
            /// </summary>
            [EnumMember(Value = "loan")]
            Loan
        }

        /// <summary>
        /// Type of the contract
        /// </summary>
        /// <value>Type of the contract</value>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public TypeEnum? Type { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetContractsPublicRegionId200Ok" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetContractsPublicRegionId200Ok() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetContractsPublicRegionId200Ok" /> class.
        /// </summary>
        /// <param name="Buyout">Buyout price (for Auctions only).</param>
        /// <param name="Collateral">Collateral price (for Couriers only).</param>
        /// <param name="ContractId">contract_id integer (required).</param>
        /// <param name="DateExpired">Expiration date of the contract (required).</param>
        /// <param name="DateIssued">Сreation date of the contract (required).</param>
        /// <param name="DaysToComplete">Number of days to perform the contract.</param>
        /// <param name="EndLocationId">End location ID (for Couriers contract).</param>
        /// <param name="ForCorporation">true if the contract was issued on behalf of the issuer&#39;s corporation.</param>
        /// <param name="IssuerCorporationId">Character&#39;s corporation ID for the issuer (required).</param>
        /// <param name="IssuerId">Character ID for the issuer (required).</param>
        /// <param name="Price">Price of contract (for ItemsExchange and Auctions).</param>
        /// <param name="Reward">Remuneration for contract (for Couriers only).</param>
        /// <param name="StartLocationId">Start location ID (for Couriers contract).</param>
        /// <param name="Title">Title of the contract.</param>
        /// <param name="Type">Type of the contract (required).</param>
        /// <param name="Volume">Volume of items in the contract.</param>
        public GetContractsPublicRegionId200Ok(double? Buyout = default(double?), double? Collateral = default(double?), int? ContractId = default(int?), DateTime? DateExpired = default(DateTime?), DateTime? DateIssued = default(DateTime?), int? DaysToComplete = default(int?), long? EndLocationId = default(long?), bool? ForCorporation = default(bool?), int? IssuerCorporationId = default(int?), int? IssuerId = default(int?), double? Price = default(double?), double? Reward = default(double?), long? StartLocationId = default(long?), string Title = default(string), TypeEnum? Type = default(TypeEnum?), double? Volume = default(double?))
        {
            // to ensure "ContractId" is required (not null)
            if (ContractId == null)
            {
                throw new InvalidDataException("ContractId is a required property for GetContractsPublicRegionId200Ok and cannot be null");
            }
            else
            {
                this.ContractId = ContractId;
            }
            // to ensure "DateExpired" is required (not null)
            if (DateExpired == null)
            {
                throw new InvalidDataException("DateExpired is a required property for GetContractsPublicRegionId200Ok and cannot be null");
            }
            else
            {
                this.DateExpired = DateExpired;
            }
            // to ensure "DateIssued" is required (not null)
            if (DateIssued == null)
            {
                throw new InvalidDataException("DateIssued is a required property for GetContractsPublicRegionId200Ok and cannot be null");
            }
            else
            {
                this.DateIssued = DateIssued;
            }
            // to ensure "IssuerCorporationId" is required (not null)
            if (IssuerCorporationId == null)
            {
                throw new InvalidDataException("IssuerCorporationId is a required property for GetContractsPublicRegionId200Ok and cannot be null");
            }
            else
            {
                this.IssuerCorporationId = IssuerCorporationId;
            }
            // to ensure "IssuerId" is required (not null)
            if (IssuerId == null)
            {
                throw new InvalidDataException("IssuerId is a required property for GetContractsPublicRegionId200Ok and cannot be null");
            }
            else
            {
                this.IssuerId = IssuerId;
            }
            // to ensure "Type" is required (not null)
            if (Type == null)
            {
                throw new InvalidDataException("Type is a required property for GetContractsPublicRegionId200Ok and cannot be null");
            }
            else
            {
                this.Type = Type;
            }
            this.Buyout = Buyout;
            this.Collateral = Collateral;
            this.DaysToComplete = DaysToComplete;
            this.EndLocationId = EndLocationId;
            this.ForCorporation = ForCorporation;
            this.Price = Price;
            this.Reward = Reward;
            this.StartLocationId = StartLocationId;
            this.Title = Title;
            this.Volume = Volume;
        }
        
        /// <summary>
        /// Buyout price (for Auctions only)
        /// </summary>
        /// <value>Buyout price (for Auctions only)</value>
        [DataMember(Name="buyout", EmitDefaultValue=false)]
        public double? Buyout { get; set; }

        /// <summary>
        /// Collateral price (for Couriers only)
        /// </summary>
        /// <value>Collateral price (for Couriers only)</value>
        [DataMember(Name="collateral", EmitDefaultValue=false)]
        public double? Collateral { get; set; }

        /// <summary>
        /// contract_id integer
        /// </summary>
        /// <value>contract_id integer</value>
        [DataMember(Name="contract_id", EmitDefaultValue=false)]
        public int? ContractId { get; set; }

        /// <summary>
        /// Expiration date of the contract
        /// </summary>
        /// <value>Expiration date of the contract</value>
        [DataMember(Name="date_expired", EmitDefaultValue=false)]
        public DateTime? DateExpired { get; set; }

        /// <summary>
        /// Сreation date of the contract
        /// </summary>
        /// <value>Сreation date of the contract</value>
        [DataMember(Name="date_issued", EmitDefaultValue=false)]
        public DateTime? DateIssued { get; set; }

        /// <summary>
        /// Number of days to perform the contract
        /// </summary>
        /// <value>Number of days to perform the contract</value>
        [DataMember(Name="days_to_complete", EmitDefaultValue=false)]
        public int? DaysToComplete { get; set; }

        /// <summary>
        /// End location ID (for Couriers contract)
        /// </summary>
        /// <value>End location ID (for Couriers contract)</value>
        [DataMember(Name="end_location_id", EmitDefaultValue=false)]
        public long? EndLocationId { get; set; }

        /// <summary>
        /// true if the contract was issued on behalf of the issuer&#39;s corporation
        /// </summary>
        /// <value>true if the contract was issued on behalf of the issuer&#39;s corporation</value>
        [DataMember(Name="for_corporation", EmitDefaultValue=false)]
        public bool? ForCorporation { get; set; }

        /// <summary>
        /// Character&#39;s corporation ID for the issuer
        /// </summary>
        /// <value>Character&#39;s corporation ID for the issuer</value>
        [DataMember(Name="issuer_corporation_id", EmitDefaultValue=false)]
        public int? IssuerCorporationId { get; set; }

        /// <summary>
        /// Character ID for the issuer
        /// </summary>
        /// <value>Character ID for the issuer</value>
        [DataMember(Name="issuer_id", EmitDefaultValue=false)]
        public int? IssuerId { get; set; }

        /// <summary>
        /// Price of contract (for ItemsExchange and Auctions)
        /// </summary>
        /// <value>Price of contract (for ItemsExchange and Auctions)</value>
        [DataMember(Name="price", EmitDefaultValue=false)]
        public double? Price { get; set; }

        /// <summary>
        /// Remuneration for contract (for Couriers only)
        /// </summary>
        /// <value>Remuneration for contract (for Couriers only)</value>
        [DataMember(Name="reward", EmitDefaultValue=false)]
        public double? Reward { get; set; }

        /// <summary>
        /// Start location ID (for Couriers contract)
        /// </summary>
        /// <value>Start location ID (for Couriers contract)</value>
        [DataMember(Name="start_location_id", EmitDefaultValue=false)]
        public long? StartLocationId { get; set; }

        /// <summary>
        /// Title of the contract
        /// </summary>
        /// <value>Title of the contract</value>
        [DataMember(Name="title", EmitDefaultValue=false)]
        public string Title { get; set; }


        /// <summary>
        /// Volume of items in the contract
        /// </summary>
        /// <value>Volume of items in the contract</value>
        [DataMember(Name="volume", EmitDefaultValue=false)]
        public double? Volume { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetContractsPublicRegionId200Ok {\n");
            sb.Append("  Buyout: ").Append(Buyout).Append("\n");
            sb.Append("  Collateral: ").Append(Collateral).Append("\n");
            sb.Append("  ContractId: ").Append(ContractId).Append("\n");
            sb.Append("  DateExpired: ").Append(DateExpired).Append("\n");
            sb.Append("  DateIssued: ").Append(DateIssued).Append("\n");
            sb.Append("  DaysToComplete: ").Append(DaysToComplete).Append("\n");
            sb.Append("  EndLocationId: ").Append(EndLocationId).Append("\n");
            sb.Append("  ForCorporation: ").Append(ForCorporation).Append("\n");
            sb.Append("  IssuerCorporationId: ").Append(IssuerCorporationId).Append("\n");
            sb.Append("  IssuerId: ").Append(IssuerId).Append("\n");
            sb.Append("  Price: ").Append(Price).Append("\n");
            sb.Append("  Reward: ").Append(Reward).Append("\n");
            sb.Append("  StartLocationId: ").Append(StartLocationId).Append("\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Volume: ").Append(Volume).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as GetContractsPublicRegionId200Ok);
        }

        /// <summary>
        /// Returns true if GetContractsPublicRegionId200Ok instances are equal
        /// </summary>
        /// <param name="input">Instance of GetContractsPublicRegionId200Ok to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetContractsPublicRegionId200Ok input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Buyout == input.Buyout ||
                    (this.Buyout != null &&
                    this.Buyout.Equals(input.Buyout))
                ) && 
                (
                    this.Collateral == input.Collateral ||
                    (this.Collateral != null &&
                    this.Collateral.Equals(input.Collateral))
                ) && 
                (
                    this.ContractId == input.ContractId ||
                    (this.ContractId != null &&
                    this.ContractId.Equals(input.ContractId))
                ) && 
                (
                    this.DateExpired == input.DateExpired ||
                    (this.DateExpired != null &&
                    this.DateExpired.Equals(input.DateExpired))
                ) && 
                (
                    this.DateIssued == input.DateIssued ||
                    (this.DateIssued != null &&
                    this.DateIssued.Equals(input.DateIssued))
                ) && 
                (
                    this.DaysToComplete == input.DaysToComplete ||
                    (this.DaysToComplete != null &&
                    this.DaysToComplete.Equals(input.DaysToComplete))
                ) && 
                (
                    this.EndLocationId == input.EndLocationId ||
                    (this.EndLocationId != null &&
                    this.EndLocationId.Equals(input.EndLocationId))
                ) && 
                (
                    this.ForCorporation == input.ForCorporation ||
                    (this.ForCorporation != null &&
                    this.ForCorporation.Equals(input.ForCorporation))
                ) && 
                (
                    this.IssuerCorporationId == input.IssuerCorporationId ||
                    (this.IssuerCorporationId != null &&
                    this.IssuerCorporationId.Equals(input.IssuerCorporationId))
                ) && 
                (
                    this.IssuerId == input.IssuerId ||
                    (this.IssuerId != null &&
                    this.IssuerId.Equals(input.IssuerId))
                ) && 
                (
                    this.Price == input.Price ||
                    (this.Price != null &&
                    this.Price.Equals(input.Price))
                ) && 
                (
                    this.Reward == input.Reward ||
                    (this.Reward != null &&
                    this.Reward.Equals(input.Reward))
                ) && 
                (
                    this.StartLocationId == input.StartLocationId ||
                    (this.StartLocationId != null &&
                    this.StartLocationId.Equals(input.StartLocationId))
                ) && 
                (
                    this.Title == input.Title ||
                    (this.Title != null &&
                    this.Title.Equals(input.Title))
                ) && 
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                ) && 
                (
                    this.Volume == input.Volume ||
                    (this.Volume != null &&
                    this.Volume.Equals(input.Volume))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Buyout != null)
                    hashCode = hashCode * 59 + this.Buyout.GetHashCode();
                if (this.Collateral != null)
                    hashCode = hashCode * 59 + this.Collateral.GetHashCode();
                if (this.ContractId != null)
                    hashCode = hashCode * 59 + this.ContractId.GetHashCode();
                if (this.DateExpired != null)
                    hashCode = hashCode * 59 + this.DateExpired.GetHashCode();
                if (this.DateIssued != null)
                    hashCode = hashCode * 59 + this.DateIssued.GetHashCode();
                if (this.DaysToComplete != null)
                    hashCode = hashCode * 59 + this.DaysToComplete.GetHashCode();
                if (this.EndLocationId != null)
                    hashCode = hashCode * 59 + this.EndLocationId.GetHashCode();
                if (this.ForCorporation != null)
                    hashCode = hashCode * 59 + this.ForCorporation.GetHashCode();
                if (this.IssuerCorporationId != null)
                    hashCode = hashCode * 59 + this.IssuerCorporationId.GetHashCode();
                if (this.IssuerId != null)
                    hashCode = hashCode * 59 + this.IssuerId.GetHashCode();
                if (this.Price != null)
                    hashCode = hashCode * 59 + this.Price.GetHashCode();
                if (this.Reward != null)
                    hashCode = hashCode * 59 + this.Reward.GetHashCode();
                if (this.StartLocationId != null)
                    hashCode = hashCode * 59 + this.StartLocationId.GetHashCode();
                if (this.Title != null)
                    hashCode = hashCode * 59 + this.Title.GetHashCode();
                if (this.Type != null)
                    hashCode = hashCode * 59 + this.Type.GetHashCode();
                if (this.Volume != null)
                    hashCode = hashCode * 59 + this.Volume.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
