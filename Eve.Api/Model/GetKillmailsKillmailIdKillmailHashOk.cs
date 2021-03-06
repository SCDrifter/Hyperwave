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
    public partial class GetKillmailsKillmailIdKillmailHashOk :  IEquatable<GetKillmailsKillmailIdKillmailHashOk>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetKillmailsKillmailIdKillmailHashOk" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetKillmailsKillmailIdKillmailHashOk() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetKillmailsKillmailIdKillmailHashOk" /> class.
        /// </summary>
        /// <param name="Attackers">attackers array (required).</param>
        /// <param name="KillmailId">ID of the killmail (required).</param>
        /// <param name="KillmailTime">Time that the victim was killed and the killmail generated  (required).</param>
        /// <param name="MoonId">Moon if the kill took place at one.</param>
        /// <param name="SolarSystemId">Solar system that the kill took place in  (required).</param>
        /// <param name="Victim">Victim.</param>
        /// <param name="WarId">War if the killmail is generated in relation to an official war .</param>
        public GetKillmailsKillmailIdKillmailHashOk(List<GetKillmailsKillmailIdKillmailHashAttacker> Attackers = default(List<GetKillmailsKillmailIdKillmailHashAttacker>), int? KillmailId = default(int?), DateTime? KillmailTime = default(DateTime?), int? MoonId = default(int?), int? SolarSystemId = default(int?), GetKillmailsKillmailIdKillmailHashVictim Victim = default(GetKillmailsKillmailIdKillmailHashVictim), int? WarId = default(int?))
        {
            // to ensure "Attackers" is required (not null)
            if (Attackers == null)
            {
                throw new InvalidDataException("Attackers is a required property for GetKillmailsKillmailIdKillmailHashOk and cannot be null");
            }
            else
            {
                this.Attackers = Attackers;
            }
            // to ensure "KillmailId" is required (not null)
            if (KillmailId == null)
            {
                throw new InvalidDataException("KillmailId is a required property for GetKillmailsKillmailIdKillmailHashOk and cannot be null");
            }
            else
            {
                this.KillmailId = KillmailId;
            }
            // to ensure "KillmailTime" is required (not null)
            if (KillmailTime == null)
            {
                throw new InvalidDataException("KillmailTime is a required property for GetKillmailsKillmailIdKillmailHashOk and cannot be null");
            }
            else
            {
                this.KillmailTime = KillmailTime;
            }
            // to ensure "SolarSystemId" is required (not null)
            if (SolarSystemId == null)
            {
                throw new InvalidDataException("SolarSystemId is a required property for GetKillmailsKillmailIdKillmailHashOk and cannot be null");
            }
            else
            {
                this.SolarSystemId = SolarSystemId;
            }
            this.MoonId = MoonId;
            this.Victim = Victim;
            this.WarId = WarId;
        }
        
        /// <summary>
        /// attackers array
        /// </summary>
        /// <value>attackers array</value>
        [DataMember(Name="attackers", EmitDefaultValue=false)]
        public List<GetKillmailsKillmailIdKillmailHashAttacker> Attackers { get; set; }

        /// <summary>
        /// ID of the killmail
        /// </summary>
        /// <value>ID of the killmail</value>
        [DataMember(Name="killmail_id", EmitDefaultValue=false)]
        public int? KillmailId { get; set; }

        /// <summary>
        /// Time that the victim was killed and the killmail generated 
        /// </summary>
        /// <value>Time that the victim was killed and the killmail generated </value>
        [DataMember(Name="killmail_time", EmitDefaultValue=false)]
        public DateTime? KillmailTime { get; set; }

        /// <summary>
        /// Moon if the kill took place at one
        /// </summary>
        /// <value>Moon if the kill took place at one</value>
        [DataMember(Name="moon_id", EmitDefaultValue=false)]
        public int? MoonId { get; set; }

        /// <summary>
        /// Solar system that the kill took place in 
        /// </summary>
        /// <value>Solar system that the kill took place in </value>
        [DataMember(Name="solar_system_id", EmitDefaultValue=false)]
        public int? SolarSystemId { get; set; }

        /// <summary>
        /// Gets or Sets Victim
        /// </summary>
        [DataMember(Name="victim", EmitDefaultValue=false)]
        public GetKillmailsKillmailIdKillmailHashVictim Victim { get; set; }

        /// <summary>
        /// War if the killmail is generated in relation to an official war 
        /// </summary>
        /// <value>War if the killmail is generated in relation to an official war </value>
        [DataMember(Name="war_id", EmitDefaultValue=false)]
        public int? WarId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetKillmailsKillmailIdKillmailHashOk {\n");
            sb.Append("  Attackers: ").Append(Attackers).Append("\n");
            sb.Append("  KillmailId: ").Append(KillmailId).Append("\n");
            sb.Append("  KillmailTime: ").Append(KillmailTime).Append("\n");
            sb.Append("  MoonId: ").Append(MoonId).Append("\n");
            sb.Append("  SolarSystemId: ").Append(SolarSystemId).Append("\n");
            sb.Append("  Victim: ").Append(Victim).Append("\n");
            sb.Append("  WarId: ").Append(WarId).Append("\n");
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
            return this.Equals(input as GetKillmailsKillmailIdKillmailHashOk);
        }

        /// <summary>
        /// Returns true if GetKillmailsKillmailIdKillmailHashOk instances are equal
        /// </summary>
        /// <param name="input">Instance of GetKillmailsKillmailIdKillmailHashOk to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetKillmailsKillmailIdKillmailHashOk input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Attackers == input.Attackers ||
                    (this.Attackers != null &&
                    this.Attackers.SequenceEqual(input.Attackers))
                ) && 
                (
                    this.KillmailId == input.KillmailId ||
                    (this.KillmailId != null &&
                    this.KillmailId.Equals(input.KillmailId))
                ) && 
                (
                    this.KillmailTime == input.KillmailTime ||
                    (this.KillmailTime != null &&
                    this.KillmailTime.Equals(input.KillmailTime))
                ) && 
                (
                    this.MoonId == input.MoonId ||
                    (this.MoonId != null &&
                    this.MoonId.Equals(input.MoonId))
                ) && 
                (
                    this.SolarSystemId == input.SolarSystemId ||
                    (this.SolarSystemId != null &&
                    this.SolarSystemId.Equals(input.SolarSystemId))
                ) && 
                (
                    this.Victim == input.Victim ||
                    (this.Victim != null &&
                    this.Victim.Equals(input.Victim))
                ) && 
                (
                    this.WarId == input.WarId ||
                    (this.WarId != null &&
                    this.WarId.Equals(input.WarId))
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
                if (this.Attackers != null)
                    hashCode = hashCode * 59 + this.Attackers.GetHashCode();
                if (this.KillmailId != null)
                    hashCode = hashCode * 59 + this.KillmailId.GetHashCode();
                if (this.KillmailTime != null)
                    hashCode = hashCode * 59 + this.KillmailTime.GetHashCode();
                if (this.MoonId != null)
                    hashCode = hashCode * 59 + this.MoonId.GetHashCode();
                if (this.SolarSystemId != null)
                    hashCode = hashCode * 59 + this.SolarSystemId.GetHashCode();
                if (this.Victim != null)
                    hashCode = hashCode * 59 + this.Victim.GetHashCode();
                if (this.WarId != null)
                    hashCode = hashCode * 59 + this.WarId.GetHashCode();
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
