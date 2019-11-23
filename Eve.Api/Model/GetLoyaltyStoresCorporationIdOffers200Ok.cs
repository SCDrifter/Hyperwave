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
    public partial class GetLoyaltyStoresCorporationIdOffers200Ok :  IEquatable<GetLoyaltyStoresCorporationIdOffers200Ok>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLoyaltyStoresCorporationIdOffers200Ok" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetLoyaltyStoresCorporationIdOffers200Ok() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLoyaltyStoresCorporationIdOffers200Ok" /> class.
        /// </summary>
        /// <param name="AkCost">Analysis kredit cost.</param>
        /// <param name="IskCost">isk_cost integer (required).</param>
        /// <param name="LpCost">lp_cost integer (required).</param>
        /// <param name="OfferId">offer_id integer (required).</param>
        /// <param name="Quantity">quantity integer (required).</param>
        /// <param name="RequiredItems">required_items array (required).</param>
        /// <param name="TypeId">type_id integer (required).</param>
        public GetLoyaltyStoresCorporationIdOffers200Ok(int? AkCost = default(int?), long? IskCost = default(long?), int? LpCost = default(int?), int? OfferId = default(int?), int? Quantity = default(int?), List<GetLoyaltyStoresCorporationIdOffersRequiredItem> RequiredItems = default(List<GetLoyaltyStoresCorporationIdOffersRequiredItem>), int? TypeId = default(int?))
        {
            // to ensure "IskCost" is required (not null)
            if (IskCost == null)
            {
                throw new InvalidDataException("IskCost is a required property for GetLoyaltyStoresCorporationIdOffers200Ok and cannot be null");
            }
            else
            {
                this.IskCost = IskCost;
            }
            // to ensure "LpCost" is required (not null)
            if (LpCost == null)
            {
                throw new InvalidDataException("LpCost is a required property for GetLoyaltyStoresCorporationIdOffers200Ok and cannot be null");
            }
            else
            {
                this.LpCost = LpCost;
            }
            // to ensure "OfferId" is required (not null)
            if (OfferId == null)
            {
                throw new InvalidDataException("OfferId is a required property for GetLoyaltyStoresCorporationIdOffers200Ok and cannot be null");
            }
            else
            {
                this.OfferId = OfferId;
            }
            // to ensure "Quantity" is required (not null)
            if (Quantity == null)
            {
                throw new InvalidDataException("Quantity is a required property for GetLoyaltyStoresCorporationIdOffers200Ok and cannot be null");
            }
            else
            {
                this.Quantity = Quantity;
            }
            // to ensure "RequiredItems" is required (not null)
            if (RequiredItems == null)
            {
                throw new InvalidDataException("RequiredItems is a required property for GetLoyaltyStoresCorporationIdOffers200Ok and cannot be null");
            }
            else
            {
                this.RequiredItems = RequiredItems;
            }
            // to ensure "TypeId" is required (not null)
            if (TypeId == null)
            {
                throw new InvalidDataException("TypeId is a required property for GetLoyaltyStoresCorporationIdOffers200Ok and cannot be null");
            }
            else
            {
                this.TypeId = TypeId;
            }
            this.AkCost = AkCost;
        }
        
        /// <summary>
        /// Analysis kredit cost
        /// </summary>
        /// <value>Analysis kredit cost</value>
        [DataMember(Name="ak_cost", EmitDefaultValue=false)]
        public int? AkCost { get; set; }

        /// <summary>
        /// isk_cost integer
        /// </summary>
        /// <value>isk_cost integer</value>
        [DataMember(Name="isk_cost", EmitDefaultValue=false)]
        public long? IskCost { get; set; }

        /// <summary>
        /// lp_cost integer
        /// </summary>
        /// <value>lp_cost integer</value>
        [DataMember(Name="lp_cost", EmitDefaultValue=false)]
        public int? LpCost { get; set; }

        /// <summary>
        /// offer_id integer
        /// </summary>
        /// <value>offer_id integer</value>
        [DataMember(Name="offer_id", EmitDefaultValue=false)]
        public int? OfferId { get; set; }

        /// <summary>
        /// quantity integer
        /// </summary>
        /// <value>quantity integer</value>
        [DataMember(Name="quantity", EmitDefaultValue=false)]
        public int? Quantity { get; set; }

        /// <summary>
        /// required_items array
        /// </summary>
        /// <value>required_items array</value>
        [DataMember(Name="required_items", EmitDefaultValue=false)]
        public List<GetLoyaltyStoresCorporationIdOffersRequiredItem> RequiredItems { get; set; }

        /// <summary>
        /// type_id integer
        /// </summary>
        /// <value>type_id integer</value>
        [DataMember(Name="type_id", EmitDefaultValue=false)]
        public int? TypeId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetLoyaltyStoresCorporationIdOffers200Ok {\n");
            sb.Append("  AkCost: ").Append(AkCost).Append("\n");
            sb.Append("  IskCost: ").Append(IskCost).Append("\n");
            sb.Append("  LpCost: ").Append(LpCost).Append("\n");
            sb.Append("  OfferId: ").Append(OfferId).Append("\n");
            sb.Append("  Quantity: ").Append(Quantity).Append("\n");
            sb.Append("  RequiredItems: ").Append(RequiredItems).Append("\n");
            sb.Append("  TypeId: ").Append(TypeId).Append("\n");
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
            return this.Equals(input as GetLoyaltyStoresCorporationIdOffers200Ok);
        }

        /// <summary>
        /// Returns true if GetLoyaltyStoresCorporationIdOffers200Ok instances are equal
        /// </summary>
        /// <param name="input">Instance of GetLoyaltyStoresCorporationIdOffers200Ok to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetLoyaltyStoresCorporationIdOffers200Ok input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.AkCost == input.AkCost ||
                    (this.AkCost != null &&
                    this.AkCost.Equals(input.AkCost))
                ) && 
                (
                    this.IskCost == input.IskCost ||
                    (this.IskCost != null &&
                    this.IskCost.Equals(input.IskCost))
                ) && 
                (
                    this.LpCost == input.LpCost ||
                    (this.LpCost != null &&
                    this.LpCost.Equals(input.LpCost))
                ) && 
                (
                    this.OfferId == input.OfferId ||
                    (this.OfferId != null &&
                    this.OfferId.Equals(input.OfferId))
                ) && 
                (
                    this.Quantity == input.Quantity ||
                    (this.Quantity != null &&
                    this.Quantity.Equals(input.Quantity))
                ) && 
                (
                    this.RequiredItems == input.RequiredItems ||
                    (this.RequiredItems != null &&
                    this.RequiredItems.SequenceEqual(input.RequiredItems))
                ) && 
                (
                    this.TypeId == input.TypeId ||
                    (this.TypeId != null &&
                    this.TypeId.Equals(input.TypeId))
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
                if (this.AkCost != null)
                    hashCode = hashCode * 59 + this.AkCost.GetHashCode();
                if (this.IskCost != null)
                    hashCode = hashCode * 59 + this.IskCost.GetHashCode();
                if (this.LpCost != null)
                    hashCode = hashCode * 59 + this.LpCost.GetHashCode();
                if (this.OfferId != null)
                    hashCode = hashCode * 59 + this.OfferId.GetHashCode();
                if (this.Quantity != null)
                    hashCode = hashCode * 59 + this.Quantity.GetHashCode();
                if (this.RequiredItems != null)
                    hashCode = hashCode * 59 + this.RequiredItems.GetHashCode();
                if (this.TypeId != null)
                    hashCode = hashCode * 59 + this.TypeId.GetHashCode();
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
