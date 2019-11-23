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
    public partial class GetCorporationsCorporationIdStarbases200Ok :  IEquatable<GetCorporationsCorporationIdStarbases200Ok>, IValidatableObject
    {
        /// <summary>
        /// state string
        /// </summary>
        /// <value>state string</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum StateEnum
        {
            
            /// <summary>
            /// Enum Offline for "offline"
            /// </summary>
            [EnumMember(Value = "offline")]
            Offline,
            
            /// <summary>
            /// Enum Online for "online"
            /// </summary>
            [EnumMember(Value = "online")]
            Online,
            
            /// <summary>
            /// Enum Onlining for "onlining"
            /// </summary>
            [EnumMember(Value = "onlining")]
            Onlining,
            
            /// <summary>
            /// Enum Reinforced for "reinforced"
            /// </summary>
            [EnumMember(Value = "reinforced")]
            Reinforced,
            
            /// <summary>
            /// Enum Unanchoring for "unanchoring"
            /// </summary>
            [EnumMember(Value = "unanchoring")]
            Unanchoring
        }

        /// <summary>
        /// state string
        /// </summary>
        /// <value>state string</value>
        [DataMember(Name="state", EmitDefaultValue=false)]
        public StateEnum? State { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCorporationsCorporationIdStarbases200Ok" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetCorporationsCorporationIdStarbases200Ok() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCorporationsCorporationIdStarbases200Ok" /> class.
        /// </summary>
        /// <param name="MoonId">The moon this starbase (POS) is anchored on, unanchored POSes do not have this information.</param>
        /// <param name="OnlinedSince">When the POS onlined, for starbases (POSes) in online state.</param>
        /// <param name="ReinforcedUntil">When the POS will be out of reinforcement, for starbases (POSes) in reinforced state.</param>
        /// <param name="StarbaseId">Unique ID for this starbase (POS) (required).</param>
        /// <param name="State">state string.</param>
        /// <param name="SystemId">The solar system this starbase (POS) is in, unanchored POSes have this information (required).</param>
        /// <param name="TypeId">Starbase (POS) type (required).</param>
        /// <param name="UnanchorAt">When the POS started unanchoring, for starbases (POSes) in unanchoring state.</param>
        public GetCorporationsCorporationIdStarbases200Ok(int? MoonId = default(int?), DateTime? OnlinedSince = default(DateTime?), DateTime? ReinforcedUntil = default(DateTime?), long? StarbaseId = default(long?), StateEnum? State = default(StateEnum?), int? SystemId = default(int?), int? TypeId = default(int?), DateTime? UnanchorAt = default(DateTime?))
        {
            // to ensure "StarbaseId" is required (not null)
            if (StarbaseId == null)
            {
                throw new InvalidDataException("StarbaseId is a required property for GetCorporationsCorporationIdStarbases200Ok and cannot be null");
            }
            else
            {
                this.StarbaseId = StarbaseId;
            }
            // to ensure "SystemId" is required (not null)
            if (SystemId == null)
            {
                throw new InvalidDataException("SystemId is a required property for GetCorporationsCorporationIdStarbases200Ok and cannot be null");
            }
            else
            {
                this.SystemId = SystemId;
            }
            // to ensure "TypeId" is required (not null)
            if (TypeId == null)
            {
                throw new InvalidDataException("TypeId is a required property for GetCorporationsCorporationIdStarbases200Ok and cannot be null");
            }
            else
            {
                this.TypeId = TypeId;
            }
            this.MoonId = MoonId;
            this.OnlinedSince = OnlinedSince;
            this.ReinforcedUntil = ReinforcedUntil;
            this.State = State;
            this.UnanchorAt = UnanchorAt;
        }
        
        /// <summary>
        /// The moon this starbase (POS) is anchored on, unanchored POSes do not have this information
        /// </summary>
        /// <value>The moon this starbase (POS) is anchored on, unanchored POSes do not have this information</value>
        [DataMember(Name="moon_id", EmitDefaultValue=false)]
        public int? MoonId { get; set; }

        /// <summary>
        /// When the POS onlined, for starbases (POSes) in online state
        /// </summary>
        /// <value>When the POS onlined, for starbases (POSes) in online state</value>
        [DataMember(Name="onlined_since", EmitDefaultValue=false)]
        public DateTime? OnlinedSince { get; set; }

        /// <summary>
        /// When the POS will be out of reinforcement, for starbases (POSes) in reinforced state
        /// </summary>
        /// <value>When the POS will be out of reinforcement, for starbases (POSes) in reinforced state</value>
        [DataMember(Name="reinforced_until", EmitDefaultValue=false)]
        public DateTime? ReinforcedUntil { get; set; }

        /// <summary>
        /// Unique ID for this starbase (POS)
        /// </summary>
        /// <value>Unique ID for this starbase (POS)</value>
        [DataMember(Name="starbase_id", EmitDefaultValue=false)]
        public long? StarbaseId { get; set; }


        /// <summary>
        /// The solar system this starbase (POS) is in, unanchored POSes have this information
        /// </summary>
        /// <value>The solar system this starbase (POS) is in, unanchored POSes have this information</value>
        [DataMember(Name="system_id", EmitDefaultValue=false)]
        public int? SystemId { get; set; }

        /// <summary>
        /// Starbase (POS) type
        /// </summary>
        /// <value>Starbase (POS) type</value>
        [DataMember(Name="type_id", EmitDefaultValue=false)]
        public int? TypeId { get; set; }

        /// <summary>
        /// When the POS started unanchoring, for starbases (POSes) in unanchoring state
        /// </summary>
        /// <value>When the POS started unanchoring, for starbases (POSes) in unanchoring state</value>
        [DataMember(Name="unanchor_at", EmitDefaultValue=false)]
        public DateTime? UnanchorAt { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetCorporationsCorporationIdStarbases200Ok {\n");
            sb.Append("  MoonId: ").Append(MoonId).Append("\n");
            sb.Append("  OnlinedSince: ").Append(OnlinedSince).Append("\n");
            sb.Append("  ReinforcedUntil: ").Append(ReinforcedUntil).Append("\n");
            sb.Append("  StarbaseId: ").Append(StarbaseId).Append("\n");
            sb.Append("  State: ").Append(State).Append("\n");
            sb.Append("  SystemId: ").Append(SystemId).Append("\n");
            sb.Append("  TypeId: ").Append(TypeId).Append("\n");
            sb.Append("  UnanchorAt: ").Append(UnanchorAt).Append("\n");
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
            return this.Equals(input as GetCorporationsCorporationIdStarbases200Ok);
        }

        /// <summary>
        /// Returns true if GetCorporationsCorporationIdStarbases200Ok instances are equal
        /// </summary>
        /// <param name="input">Instance of GetCorporationsCorporationIdStarbases200Ok to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetCorporationsCorporationIdStarbases200Ok input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.MoonId == input.MoonId ||
                    (this.MoonId != null &&
                    this.MoonId.Equals(input.MoonId))
                ) && 
                (
                    this.OnlinedSince == input.OnlinedSince ||
                    (this.OnlinedSince != null &&
                    this.OnlinedSince.Equals(input.OnlinedSince))
                ) && 
                (
                    this.ReinforcedUntil == input.ReinforcedUntil ||
                    (this.ReinforcedUntil != null &&
                    this.ReinforcedUntil.Equals(input.ReinforcedUntil))
                ) && 
                (
                    this.StarbaseId == input.StarbaseId ||
                    (this.StarbaseId != null &&
                    this.StarbaseId.Equals(input.StarbaseId))
                ) && 
                (
                    this.State == input.State ||
                    (this.State != null &&
                    this.State.Equals(input.State))
                ) && 
                (
                    this.SystemId == input.SystemId ||
                    (this.SystemId != null &&
                    this.SystemId.Equals(input.SystemId))
                ) && 
                (
                    this.TypeId == input.TypeId ||
                    (this.TypeId != null &&
                    this.TypeId.Equals(input.TypeId))
                ) && 
                (
                    this.UnanchorAt == input.UnanchorAt ||
                    (this.UnanchorAt != null &&
                    this.UnanchorAt.Equals(input.UnanchorAt))
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
                if (this.MoonId != null)
                    hashCode = hashCode * 59 + this.MoonId.GetHashCode();
                if (this.OnlinedSince != null)
                    hashCode = hashCode * 59 + this.OnlinedSince.GetHashCode();
                if (this.ReinforcedUntil != null)
                    hashCode = hashCode * 59 + this.ReinforcedUntil.GetHashCode();
                if (this.StarbaseId != null)
                    hashCode = hashCode * 59 + this.StarbaseId.GetHashCode();
                if (this.State != null)
                    hashCode = hashCode * 59 + this.State.GetHashCode();
                if (this.SystemId != null)
                    hashCode = hashCode * 59 + this.SystemId.GetHashCode();
                if (this.TypeId != null)
                    hashCode = hashCode * 59 + this.TypeId.GetHashCode();
                if (this.UnanchorAt != null)
                    hashCode = hashCode * 59 + this.UnanchorAt.GetHashCode();
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
