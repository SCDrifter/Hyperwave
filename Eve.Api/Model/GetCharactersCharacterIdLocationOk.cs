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
    public partial class GetCharactersCharacterIdLocationOk :  IEquatable<GetCharactersCharacterIdLocationOk>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCharactersCharacterIdLocationOk" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetCharactersCharacterIdLocationOk() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCharactersCharacterIdLocationOk" /> class.
        /// </summary>
        /// <param name="SolarSystemId">solar_system_id integer (required).</param>
        /// <param name="StationId">station_id integer.</param>
        /// <param name="StructureId">structure_id integer.</param>
        public GetCharactersCharacterIdLocationOk(int? SolarSystemId = default(int?), int? StationId = default(int?), long? StructureId = default(long?))
        {
            // to ensure "SolarSystemId" is required (not null)
            if (SolarSystemId == null)
            {
                throw new InvalidDataException("SolarSystemId is a required property for GetCharactersCharacterIdLocationOk and cannot be null");
            }
            else
            {
                this.SolarSystemId = SolarSystemId;
            }
            this.StationId = StationId;
            this.StructureId = StructureId;
        }
        
        /// <summary>
        /// solar_system_id integer
        /// </summary>
        /// <value>solar_system_id integer</value>
        [DataMember(Name="solar_system_id", EmitDefaultValue=false)]
        public int? SolarSystemId { get; set; }

        /// <summary>
        /// station_id integer
        /// </summary>
        /// <value>station_id integer</value>
        [DataMember(Name="station_id", EmitDefaultValue=false)]
        public int? StationId { get; set; }

        /// <summary>
        /// structure_id integer
        /// </summary>
        /// <value>structure_id integer</value>
        [DataMember(Name="structure_id", EmitDefaultValue=false)]
        public long? StructureId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetCharactersCharacterIdLocationOk {\n");
            sb.Append("  SolarSystemId: ").Append(SolarSystemId).Append("\n");
            sb.Append("  StationId: ").Append(StationId).Append("\n");
            sb.Append("  StructureId: ").Append(StructureId).Append("\n");
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
            return this.Equals(input as GetCharactersCharacterIdLocationOk);
        }

        /// <summary>
        /// Returns true if GetCharactersCharacterIdLocationOk instances are equal
        /// </summary>
        /// <param name="input">Instance of GetCharactersCharacterIdLocationOk to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetCharactersCharacterIdLocationOk input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.SolarSystemId == input.SolarSystemId ||
                    (this.SolarSystemId != null &&
                    this.SolarSystemId.Equals(input.SolarSystemId))
                ) && 
                (
                    this.StationId == input.StationId ||
                    (this.StationId != null &&
                    this.StationId.Equals(input.StationId))
                ) && 
                (
                    this.StructureId == input.StructureId ||
                    (this.StructureId != null &&
                    this.StructureId.Equals(input.StructureId))
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
                if (this.SolarSystemId != null)
                    hashCode = hashCode * 59 + this.SolarSystemId.GetHashCode();
                if (this.StationId != null)
                    hashCode = hashCode * 59 + this.StationId.GetHashCode();
                if (this.StructureId != null)
                    hashCode = hashCode * 59 + this.StructureId.GetHashCode();
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
