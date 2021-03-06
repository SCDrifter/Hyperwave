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
    public partial class GetIncursions200Ok :  IEquatable<GetIncursions200Ok>, IValidatableObject
    {
        /// <summary>
        /// The state of this incursion
        /// </summary>
        /// <value>The state of this incursion</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum StateEnum
        {
            
            /// <summary>
            /// Enum Withdrawing for "withdrawing"
            /// </summary>
            [EnumMember(Value = "withdrawing")]
            Withdrawing,
            
            /// <summary>
            /// Enum Mobilizing for "mobilizing"
            /// </summary>
            [EnumMember(Value = "mobilizing")]
            Mobilizing,
            
            /// <summary>
            /// Enum Established for "established"
            /// </summary>
            [EnumMember(Value = "established")]
            Established
        }

        /// <summary>
        /// The state of this incursion
        /// </summary>
        /// <value>The state of this incursion</value>
        [DataMember(Name="state", EmitDefaultValue=false)]
        public StateEnum? State { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetIncursions200Ok" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetIncursions200Ok() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetIncursions200Ok" /> class.
        /// </summary>
        /// <param name="ConstellationId">The constellation id in which this incursion takes place (required).</param>
        /// <param name="FactionId">The attacking faction&#39;s id (required).</param>
        /// <param name="HasBoss">Whether the final encounter has boss or not (required).</param>
        /// <param name="InfestedSolarSystems">A list of infested solar system ids that are a part of this incursion (required).</param>
        /// <param name="Influence">Influence of this incursion as a float from 0 to 1 (required).</param>
        /// <param name="StagingSolarSystemId">Staging solar system for this incursion (required).</param>
        /// <param name="State">The state of this incursion (required).</param>
        /// <param name="Type">The type of this incursion (required).</param>
        public GetIncursions200Ok(int? ConstellationId = default(int?), int? FactionId = default(int?), bool? HasBoss = default(bool?), List<int?> InfestedSolarSystems = default(List<int?>), float? Influence = default(float?), int? StagingSolarSystemId = default(int?), StateEnum? State = default(StateEnum?), string Type = default(string))
        {
            // to ensure "ConstellationId" is required (not null)
            if (ConstellationId == null)
            {
                throw new InvalidDataException("ConstellationId is a required property for GetIncursions200Ok and cannot be null");
            }
            else
            {
                this.ConstellationId = ConstellationId;
            }
            // to ensure "FactionId" is required (not null)
            if (FactionId == null)
            {
                throw new InvalidDataException("FactionId is a required property for GetIncursions200Ok and cannot be null");
            }
            else
            {
                this.FactionId = FactionId;
            }
            // to ensure "HasBoss" is required (not null)
            if (HasBoss == null)
            {
                throw new InvalidDataException("HasBoss is a required property for GetIncursions200Ok and cannot be null");
            }
            else
            {
                this.HasBoss = HasBoss;
            }
            // to ensure "InfestedSolarSystems" is required (not null)
            if (InfestedSolarSystems == null)
            {
                throw new InvalidDataException("InfestedSolarSystems is a required property for GetIncursions200Ok and cannot be null");
            }
            else
            {
                this.InfestedSolarSystems = InfestedSolarSystems;
            }
            // to ensure "Influence" is required (not null)
            if (Influence == null)
            {
                throw new InvalidDataException("Influence is a required property for GetIncursions200Ok and cannot be null");
            }
            else
            {
                this.Influence = Influence;
            }
            // to ensure "StagingSolarSystemId" is required (not null)
            if (StagingSolarSystemId == null)
            {
                throw new InvalidDataException("StagingSolarSystemId is a required property for GetIncursions200Ok and cannot be null");
            }
            else
            {
                this.StagingSolarSystemId = StagingSolarSystemId;
            }
            // to ensure "State" is required (not null)
            if (State == null)
            {
                throw new InvalidDataException("State is a required property for GetIncursions200Ok and cannot be null");
            }
            else
            {
                this.State = State;
            }
            // to ensure "Type" is required (not null)
            if (Type == null)
            {
                throw new InvalidDataException("Type is a required property for GetIncursions200Ok and cannot be null");
            }
            else
            {
                this.Type = Type;
            }
        }
        
        /// <summary>
        /// The constellation id in which this incursion takes place
        /// </summary>
        /// <value>The constellation id in which this incursion takes place</value>
        [DataMember(Name="constellation_id", EmitDefaultValue=false)]
        public int? ConstellationId { get; set; }

        /// <summary>
        /// The attacking faction&#39;s id
        /// </summary>
        /// <value>The attacking faction&#39;s id</value>
        [DataMember(Name="faction_id", EmitDefaultValue=false)]
        public int? FactionId { get; set; }

        /// <summary>
        /// Whether the final encounter has boss or not
        /// </summary>
        /// <value>Whether the final encounter has boss or not</value>
        [DataMember(Name="has_boss", EmitDefaultValue=false)]
        public bool? HasBoss { get; set; }

        /// <summary>
        /// A list of infested solar system ids that are a part of this incursion
        /// </summary>
        /// <value>A list of infested solar system ids that are a part of this incursion</value>
        [DataMember(Name="infested_solar_systems", EmitDefaultValue=false)]
        public List<int?> InfestedSolarSystems { get; set; }

        /// <summary>
        /// Influence of this incursion as a float from 0 to 1
        /// </summary>
        /// <value>Influence of this incursion as a float from 0 to 1</value>
        [DataMember(Name="influence", EmitDefaultValue=false)]
        public float? Influence { get; set; }

        /// <summary>
        /// Staging solar system for this incursion
        /// </summary>
        /// <value>Staging solar system for this incursion</value>
        [DataMember(Name="staging_solar_system_id", EmitDefaultValue=false)]
        public int? StagingSolarSystemId { get; set; }


        /// <summary>
        /// The type of this incursion
        /// </summary>
        /// <value>The type of this incursion</value>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetIncursions200Ok {\n");
            sb.Append("  ConstellationId: ").Append(ConstellationId).Append("\n");
            sb.Append("  FactionId: ").Append(FactionId).Append("\n");
            sb.Append("  HasBoss: ").Append(HasBoss).Append("\n");
            sb.Append("  InfestedSolarSystems: ").Append(InfestedSolarSystems).Append("\n");
            sb.Append("  Influence: ").Append(Influence).Append("\n");
            sb.Append("  StagingSolarSystemId: ").Append(StagingSolarSystemId).Append("\n");
            sb.Append("  State: ").Append(State).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
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
            return this.Equals(input as GetIncursions200Ok);
        }

        /// <summary>
        /// Returns true if GetIncursions200Ok instances are equal
        /// </summary>
        /// <param name="input">Instance of GetIncursions200Ok to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetIncursions200Ok input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.ConstellationId == input.ConstellationId ||
                    (this.ConstellationId != null &&
                    this.ConstellationId.Equals(input.ConstellationId))
                ) && 
                (
                    this.FactionId == input.FactionId ||
                    (this.FactionId != null &&
                    this.FactionId.Equals(input.FactionId))
                ) && 
                (
                    this.HasBoss == input.HasBoss ||
                    (this.HasBoss != null &&
                    this.HasBoss.Equals(input.HasBoss))
                ) && 
                (
                    this.InfestedSolarSystems == input.InfestedSolarSystems ||
                    (this.InfestedSolarSystems != null &&
                    this.InfestedSolarSystems.SequenceEqual(input.InfestedSolarSystems))
                ) && 
                (
                    this.Influence == input.Influence ||
                    (this.Influence != null &&
                    this.Influence.Equals(input.Influence))
                ) && 
                (
                    this.StagingSolarSystemId == input.StagingSolarSystemId ||
                    (this.StagingSolarSystemId != null &&
                    this.StagingSolarSystemId.Equals(input.StagingSolarSystemId))
                ) && 
                (
                    this.State == input.State ||
                    (this.State != null &&
                    this.State.Equals(input.State))
                ) && 
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
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
                if (this.ConstellationId != null)
                    hashCode = hashCode * 59 + this.ConstellationId.GetHashCode();
                if (this.FactionId != null)
                    hashCode = hashCode * 59 + this.FactionId.GetHashCode();
                if (this.HasBoss != null)
                    hashCode = hashCode * 59 + this.HasBoss.GetHashCode();
                if (this.InfestedSolarSystems != null)
                    hashCode = hashCode * 59 + this.InfestedSolarSystems.GetHashCode();
                if (this.Influence != null)
                    hashCode = hashCode * 59 + this.Influence.GetHashCode();
                if (this.StagingSolarSystemId != null)
                    hashCode = hashCode * 59 + this.StagingSolarSystemId.GetHashCode();
                if (this.State != null)
                    hashCode = hashCode * 59 + this.State.GetHashCode();
                if (this.Type != null)
                    hashCode = hashCode * 59 + this.Type.GetHashCode();
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
