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
    /// The defending corporation or alliance that declared this war, only contains either corporation_id or alliance_id
    /// </summary>
    [DataContract]
    public partial class GetWarsWarIdDefender :  IEquatable<GetWarsWarIdDefender>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetWarsWarIdDefender" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetWarsWarIdDefender() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetWarsWarIdDefender" /> class.
        /// </summary>
        /// <param name="AllianceId">Alliance ID if and only if the defender is an alliance.</param>
        /// <param name="CorporationId">Corporation ID if and only if the defender is a corporation.</param>
        /// <param name="IskDestroyed">ISK value of ships the defender has killed (required).</param>
        /// <param name="ShipsKilled">The number of ships the defender has killed (required).</param>
        public GetWarsWarIdDefender(int? AllianceId = default(int?), int? CorporationId = default(int?), float? IskDestroyed = default(float?), int? ShipsKilled = default(int?))
        {
            // to ensure "IskDestroyed" is required (not null)
            if (IskDestroyed == null)
            {
                throw new InvalidDataException("IskDestroyed is a required property for GetWarsWarIdDefender and cannot be null");
            }
            else
            {
                this.IskDestroyed = IskDestroyed;
            }
            // to ensure "ShipsKilled" is required (not null)
            if (ShipsKilled == null)
            {
                throw new InvalidDataException("ShipsKilled is a required property for GetWarsWarIdDefender and cannot be null");
            }
            else
            {
                this.ShipsKilled = ShipsKilled;
            }
            this.AllianceId = AllianceId;
            this.CorporationId = CorporationId;
        }
        
        /// <summary>
        /// Alliance ID if and only if the defender is an alliance
        /// </summary>
        /// <value>Alliance ID if and only if the defender is an alliance</value>
        [DataMember(Name="alliance_id", EmitDefaultValue=false)]
        public int? AllianceId { get; set; }

        /// <summary>
        /// Corporation ID if and only if the defender is a corporation
        /// </summary>
        /// <value>Corporation ID if and only if the defender is a corporation</value>
        [DataMember(Name="corporation_id", EmitDefaultValue=false)]
        public int? CorporationId { get; set; }

        /// <summary>
        /// ISK value of ships the defender has killed
        /// </summary>
        /// <value>ISK value of ships the defender has killed</value>
        [DataMember(Name="isk_destroyed", EmitDefaultValue=false)]
        public float? IskDestroyed { get; set; }

        /// <summary>
        /// The number of ships the defender has killed
        /// </summary>
        /// <value>The number of ships the defender has killed</value>
        [DataMember(Name="ships_killed", EmitDefaultValue=false)]
        public int? ShipsKilled { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetWarsWarIdDefender {\n");
            sb.Append("  AllianceId: ").Append(AllianceId).Append("\n");
            sb.Append("  CorporationId: ").Append(CorporationId).Append("\n");
            sb.Append("  IskDestroyed: ").Append(IskDestroyed).Append("\n");
            sb.Append("  ShipsKilled: ").Append(ShipsKilled).Append("\n");
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
            return this.Equals(input as GetWarsWarIdDefender);
        }

        /// <summary>
        /// Returns true if GetWarsWarIdDefender instances are equal
        /// </summary>
        /// <param name="input">Instance of GetWarsWarIdDefender to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetWarsWarIdDefender input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.AllianceId == input.AllianceId ||
                    (this.AllianceId != null &&
                    this.AllianceId.Equals(input.AllianceId))
                ) && 
                (
                    this.CorporationId == input.CorporationId ||
                    (this.CorporationId != null &&
                    this.CorporationId.Equals(input.CorporationId))
                ) && 
                (
                    this.IskDestroyed == input.IskDestroyed ||
                    (this.IskDestroyed != null &&
                    this.IskDestroyed.Equals(input.IskDestroyed))
                ) && 
                (
                    this.ShipsKilled == input.ShipsKilled ||
                    (this.ShipsKilled != null &&
                    this.ShipsKilled.Equals(input.ShipsKilled))
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
                if (this.AllianceId != null)
                    hashCode = hashCode * 59 + this.AllianceId.GetHashCode();
                if (this.CorporationId != null)
                    hashCode = hashCode * 59 + this.CorporationId.GetHashCode();
                if (this.IskDestroyed != null)
                    hashCode = hashCode * 59 + this.IskDestroyed.GetHashCode();
                if (this.ShipsKilled != null)
                    hashCode = hashCode * 59 + this.ShipsKilled.GetHashCode();
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
