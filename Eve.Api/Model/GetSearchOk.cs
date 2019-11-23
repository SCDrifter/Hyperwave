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
    public partial class GetSearchOk :  IEquatable<GetSearchOk>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSearchOk" /> class.
        /// </summary>
        /// <param name="Agent">agent array.</param>
        /// <param name="Alliance">alliance array.</param>
        /// <param name="Character">character array.</param>
        /// <param name="Constellation">constellation array.</param>
        /// <param name="Corporation">corporation array.</param>
        /// <param name="Faction">faction array.</param>
        /// <param name="InventoryType">inventory_type array.</param>
        /// <param name="Region">region array.</param>
        /// <param name="SolarSystem">solar_system array.</param>
        /// <param name="Station">station array.</param>
        public GetSearchOk(List<int?> Agent = default(List<int?>), List<int?> Alliance = default(List<int?>), List<int?> Character = default(List<int?>), List<int?> Constellation = default(List<int?>), List<int?> Corporation = default(List<int?>), List<int?> Faction = default(List<int?>), List<int?> InventoryType = default(List<int?>), List<int?> Region = default(List<int?>), List<int?> SolarSystem = default(List<int?>), List<int?> Station = default(List<int?>))
        {
            this.Agent = Agent;
            this.Alliance = Alliance;
            this.Character = Character;
            this.Constellation = Constellation;
            this.Corporation = Corporation;
            this.Faction = Faction;
            this.InventoryType = InventoryType;
            this.Region = Region;
            this.SolarSystem = SolarSystem;
            this.Station = Station;
        }
        
        /// <summary>
        /// agent array
        /// </summary>
        /// <value>agent array</value>
        [DataMember(Name="agent", EmitDefaultValue=false)]
        public List<int?> Agent { get; set; }

        /// <summary>
        /// alliance array
        /// </summary>
        /// <value>alliance array</value>
        [DataMember(Name="alliance", EmitDefaultValue=false)]
        public List<int?> Alliance { get; set; }

        /// <summary>
        /// character array
        /// </summary>
        /// <value>character array</value>
        [DataMember(Name="character", EmitDefaultValue=false)]
        public List<int?> Character { get; set; }

        /// <summary>
        /// constellation array
        /// </summary>
        /// <value>constellation array</value>
        [DataMember(Name="constellation", EmitDefaultValue=false)]
        public List<int?> Constellation { get; set; }

        /// <summary>
        /// corporation array
        /// </summary>
        /// <value>corporation array</value>
        [DataMember(Name="corporation", EmitDefaultValue=false)]
        public List<int?> Corporation { get; set; }

        /// <summary>
        /// faction array
        /// </summary>
        /// <value>faction array</value>
        [DataMember(Name="faction", EmitDefaultValue=false)]
        public List<int?> Faction { get; set; }

        /// <summary>
        /// inventory_type array
        /// </summary>
        /// <value>inventory_type array</value>
        [DataMember(Name="inventory_type", EmitDefaultValue=false)]
        public List<int?> InventoryType { get; set; }

        /// <summary>
        /// region array
        /// </summary>
        /// <value>region array</value>
        [DataMember(Name="region", EmitDefaultValue=false)]
        public List<int?> Region { get; set; }

        /// <summary>
        /// solar_system array
        /// </summary>
        /// <value>solar_system array</value>
        [DataMember(Name="solar_system", EmitDefaultValue=false)]
        public List<int?> SolarSystem { get; set; }

        /// <summary>
        /// station array
        /// </summary>
        /// <value>station array</value>
        [DataMember(Name="station", EmitDefaultValue=false)]
        public List<int?> Station { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetSearchOk {\n");
            sb.Append("  Agent: ").Append(Agent).Append("\n");
            sb.Append("  Alliance: ").Append(Alliance).Append("\n");
            sb.Append("  Character: ").Append(Character).Append("\n");
            sb.Append("  Constellation: ").Append(Constellation).Append("\n");
            sb.Append("  Corporation: ").Append(Corporation).Append("\n");
            sb.Append("  Faction: ").Append(Faction).Append("\n");
            sb.Append("  InventoryType: ").Append(InventoryType).Append("\n");
            sb.Append("  Region: ").Append(Region).Append("\n");
            sb.Append("  SolarSystem: ").Append(SolarSystem).Append("\n");
            sb.Append("  Station: ").Append(Station).Append("\n");
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
            return this.Equals(input as GetSearchOk);
        }

        /// <summary>
        /// Returns true if GetSearchOk instances are equal
        /// </summary>
        /// <param name="input">Instance of GetSearchOk to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetSearchOk input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Agent == input.Agent ||
                    (this.Agent != null &&
                    this.Agent.SequenceEqual(input.Agent))
                ) && 
                (
                    this.Alliance == input.Alliance ||
                    (this.Alliance != null &&
                    this.Alliance.SequenceEqual(input.Alliance))
                ) && 
                (
                    this.Character == input.Character ||
                    (this.Character != null &&
                    this.Character.SequenceEqual(input.Character))
                ) && 
                (
                    this.Constellation == input.Constellation ||
                    (this.Constellation != null &&
                    this.Constellation.SequenceEqual(input.Constellation))
                ) && 
                (
                    this.Corporation == input.Corporation ||
                    (this.Corporation != null &&
                    this.Corporation.SequenceEqual(input.Corporation))
                ) && 
                (
                    this.Faction == input.Faction ||
                    (this.Faction != null &&
                    this.Faction.SequenceEqual(input.Faction))
                ) && 
                (
                    this.InventoryType == input.InventoryType ||
                    (this.InventoryType != null &&
                    this.InventoryType.SequenceEqual(input.InventoryType))
                ) && 
                (
                    this.Region == input.Region ||
                    (this.Region != null &&
                    this.Region.SequenceEqual(input.Region))
                ) && 
                (
                    this.SolarSystem == input.SolarSystem ||
                    (this.SolarSystem != null &&
                    this.SolarSystem.SequenceEqual(input.SolarSystem))
                ) && 
                (
                    this.Station == input.Station ||
                    (this.Station != null &&
                    this.Station.SequenceEqual(input.Station))
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
                if (this.Agent != null)
                    hashCode = hashCode * 59 + this.Agent.GetHashCode();
                if (this.Alliance != null)
                    hashCode = hashCode * 59 + this.Alliance.GetHashCode();
                if (this.Character != null)
                    hashCode = hashCode * 59 + this.Character.GetHashCode();
                if (this.Constellation != null)
                    hashCode = hashCode * 59 + this.Constellation.GetHashCode();
                if (this.Corporation != null)
                    hashCode = hashCode * 59 + this.Corporation.GetHashCode();
                if (this.Faction != null)
                    hashCode = hashCode * 59 + this.Faction.GetHashCode();
                if (this.InventoryType != null)
                    hashCode = hashCode * 59 + this.InventoryType.GetHashCode();
                if (this.Region != null)
                    hashCode = hashCode * 59 + this.Region.GetHashCode();
                if (this.SolarSystem != null)
                    hashCode = hashCode * 59 + this.SolarSystem.GetHashCode();
                if (this.Station != null)
                    hashCode = hashCode * 59 + this.Station.GetHashCode();
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
