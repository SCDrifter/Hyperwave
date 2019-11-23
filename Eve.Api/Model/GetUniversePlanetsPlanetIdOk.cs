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
    public partial class GetUniversePlanetsPlanetIdOk :  IEquatable<GetUniversePlanetsPlanetIdOk>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUniversePlanetsPlanetIdOk" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetUniversePlanetsPlanetIdOk() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUniversePlanetsPlanetIdOk" /> class.
        /// </summary>
        /// <param name="Name">name string (required).</param>
        /// <param name="PlanetId">planet_id integer (required).</param>
        /// <param name="Position">Position.</param>
        /// <param name="SystemId">The solar system this planet is in (required).</param>
        /// <param name="TypeId">type_id integer (required).</param>
        public GetUniversePlanetsPlanetIdOk(string Name = default(string), int? PlanetId = default(int?), GetUniversePlanetsPlanetIdPosition Position = default(GetUniversePlanetsPlanetIdPosition), int? SystemId = default(int?), int? TypeId = default(int?))
        {
            // to ensure "Name" is required (not null)
            if (Name == null)
            {
                throw new InvalidDataException("Name is a required property for GetUniversePlanetsPlanetIdOk and cannot be null");
            }
            else
            {
                this.Name = Name;
            }
            // to ensure "PlanetId" is required (not null)
            if (PlanetId == null)
            {
                throw new InvalidDataException("PlanetId is a required property for GetUniversePlanetsPlanetIdOk and cannot be null");
            }
            else
            {
                this.PlanetId = PlanetId;
            }
            // to ensure "SystemId" is required (not null)
            if (SystemId == null)
            {
                throw new InvalidDataException("SystemId is a required property for GetUniversePlanetsPlanetIdOk and cannot be null");
            }
            else
            {
                this.SystemId = SystemId;
            }
            // to ensure "TypeId" is required (not null)
            if (TypeId == null)
            {
                throw new InvalidDataException("TypeId is a required property for GetUniversePlanetsPlanetIdOk and cannot be null");
            }
            else
            {
                this.TypeId = TypeId;
            }
            this.Position = Position;
        }
        
        /// <summary>
        /// name string
        /// </summary>
        /// <value>name string</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// planet_id integer
        /// </summary>
        /// <value>planet_id integer</value>
        [DataMember(Name="planet_id", EmitDefaultValue=false)]
        public int? PlanetId { get; set; }

        /// <summary>
        /// Gets or Sets Position
        /// </summary>
        [DataMember(Name="position", EmitDefaultValue=false)]
        public GetUniversePlanetsPlanetIdPosition Position { get; set; }

        /// <summary>
        /// The solar system this planet is in
        /// </summary>
        /// <value>The solar system this planet is in</value>
        [DataMember(Name="system_id", EmitDefaultValue=false)]
        public int? SystemId { get; set; }

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
            sb.Append("class GetUniversePlanetsPlanetIdOk {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  PlanetId: ").Append(PlanetId).Append("\n");
            sb.Append("  Position: ").Append(Position).Append("\n");
            sb.Append("  SystemId: ").Append(SystemId).Append("\n");
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
            return this.Equals(input as GetUniversePlanetsPlanetIdOk);
        }

        /// <summary>
        /// Returns true if GetUniversePlanetsPlanetIdOk instances are equal
        /// </summary>
        /// <param name="input">Instance of GetUniversePlanetsPlanetIdOk to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetUniversePlanetsPlanetIdOk input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.PlanetId == input.PlanetId ||
                    (this.PlanetId != null &&
                    this.PlanetId.Equals(input.PlanetId))
                ) && 
                (
                    this.Position == input.Position ||
                    (this.Position != null &&
                    this.Position.Equals(input.Position))
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
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.PlanetId != null)
                    hashCode = hashCode * 59 + this.PlanetId.GetHashCode();
                if (this.Position != null)
                    hashCode = hashCode * 59 + this.Position.GetHashCode();
                if (this.SystemId != null)
                    hashCode = hashCode * 59 + this.SystemId.GetHashCode();
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
