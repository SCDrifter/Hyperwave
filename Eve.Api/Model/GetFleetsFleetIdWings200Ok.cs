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
    public partial class GetFleetsFleetIdWings200Ok :  IEquatable<GetFleetsFleetIdWings200Ok>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFleetsFleetIdWings200Ok" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetFleetsFleetIdWings200Ok() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFleetsFleetIdWings200Ok" /> class.
        /// </summary>
        /// <param name="Id">id integer (required).</param>
        /// <param name="Name">name string (required).</param>
        /// <param name="Squads">squads array (required).</param>
        public GetFleetsFleetIdWings200Ok(long? Id = default(long?), string Name = default(string), List<GetFleetsFleetIdWingsSquad> Squads = default(List<GetFleetsFleetIdWingsSquad>))
        {
            // to ensure "Id" is required (not null)
            if (Id == null)
            {
                throw new InvalidDataException("Id is a required property for GetFleetsFleetIdWings200Ok and cannot be null");
            }
            else
            {
                this.Id = Id;
            }
            // to ensure "Name" is required (not null)
            if (Name == null)
            {
                throw new InvalidDataException("Name is a required property for GetFleetsFleetIdWings200Ok and cannot be null");
            }
            else
            {
                this.Name = Name;
            }
            // to ensure "Squads" is required (not null)
            if (Squads == null)
            {
                throw new InvalidDataException("Squads is a required property for GetFleetsFleetIdWings200Ok and cannot be null");
            }
            else
            {
                this.Squads = Squads;
            }
        }
        
        /// <summary>
        /// id integer
        /// </summary>
        /// <value>id integer</value>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public long? Id { get; set; }

        /// <summary>
        /// name string
        /// </summary>
        /// <value>name string</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// squads array
        /// </summary>
        /// <value>squads array</value>
        [DataMember(Name="squads", EmitDefaultValue=false)]
        public List<GetFleetsFleetIdWingsSquad> Squads { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetFleetsFleetIdWings200Ok {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Squads: ").Append(Squads).Append("\n");
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
            return this.Equals(input as GetFleetsFleetIdWings200Ok);
        }

        /// <summary>
        /// Returns true if GetFleetsFleetIdWings200Ok instances are equal
        /// </summary>
        /// <param name="input">Instance of GetFleetsFleetIdWings200Ok to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetFleetsFleetIdWings200Ok input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                    this.Id.Equals(input.Id))
                ) && 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.Squads == input.Squads ||
                    (this.Squads != null &&
                    this.Squads.SequenceEqual(input.Squads))
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
                if (this.Id != null)
                    hashCode = hashCode * 59 + this.Id.GetHashCode();
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.Squads != null)
                    hashCode = hashCode * 59 + this.Squads.GetHashCode();
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
