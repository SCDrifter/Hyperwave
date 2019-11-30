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
    public partial class GetCharactersCharacterIdFittings200Ok :  IEquatable<GetCharactersCharacterIdFittings200Ok>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCharactersCharacterIdFittings200Ok" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetCharactersCharacterIdFittings200Ok() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCharactersCharacterIdFittings200Ok" /> class.
        /// </summary>
        /// <param name="Description">description string (required).</param>
        /// <param name="FittingId">fitting_id integer (required).</param>
        /// <param name="Items">items array (required).</param>
        /// <param name="Name">name string (required).</param>
        /// <param name="ShipTypeId">ship_type_id integer (required).</param>
        public GetCharactersCharacterIdFittings200Ok(string Description = default(string), int? FittingId = default(int?), List<GetCharactersCharacterIdFittingsItem> Items = default(List<GetCharactersCharacterIdFittingsItem>), string Name = default(string), int? ShipTypeId = default(int?))
        {
            // to ensure "Description" is required (not null)
            if (Description == null)
            {
                throw new InvalidDataException("Description is a required property for GetCharactersCharacterIdFittings200Ok and cannot be null");
            }
            else
            {
                this.Description = Description;
            }
            // to ensure "FittingId" is required (not null)
            if (FittingId == null)
            {
                throw new InvalidDataException("FittingId is a required property for GetCharactersCharacterIdFittings200Ok and cannot be null");
            }
            else
            {
                this.FittingId = FittingId;
            }
            // to ensure "Items" is required (not null)
            if (Items == null)
            {
                throw new InvalidDataException("Items is a required property for GetCharactersCharacterIdFittings200Ok and cannot be null");
            }
            else
            {
                this.Items = Items;
            }
            // to ensure "Name" is required (not null)
            if (Name == null)
            {
                throw new InvalidDataException("Name is a required property for GetCharactersCharacterIdFittings200Ok and cannot be null");
            }
            else
            {
                this.Name = Name;
            }
            // to ensure "ShipTypeId" is required (not null)
            if (ShipTypeId == null)
            {
                throw new InvalidDataException("ShipTypeId is a required property for GetCharactersCharacterIdFittings200Ok and cannot be null");
            }
            else
            {
                this.ShipTypeId = ShipTypeId;
            }
        }
        
        /// <summary>
        /// description string
        /// </summary>
        /// <value>description string</value>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// fitting_id integer
        /// </summary>
        /// <value>fitting_id integer</value>
        [DataMember(Name="fitting_id", EmitDefaultValue=false)]
        public int? FittingId { get; set; }

        /// <summary>
        /// items array
        /// </summary>
        /// <value>items array</value>
        [DataMember(Name="items", EmitDefaultValue=false)]
        public List<GetCharactersCharacterIdFittingsItem> Items { get; set; }

        /// <summary>
        /// name string
        /// </summary>
        /// <value>name string</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// ship_type_id integer
        /// </summary>
        /// <value>ship_type_id integer</value>
        [DataMember(Name="ship_type_id", EmitDefaultValue=false)]
        public int? ShipTypeId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetCharactersCharacterIdFittings200Ok {\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  FittingId: ").Append(FittingId).Append("\n");
            sb.Append("  Items: ").Append(Items).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  ShipTypeId: ").Append(ShipTypeId).Append("\n");
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
            return this.Equals(input as GetCharactersCharacterIdFittings200Ok);
        }

        /// <summary>
        /// Returns true if GetCharactersCharacterIdFittings200Ok instances are equal
        /// </summary>
        /// <param name="input">Instance of GetCharactersCharacterIdFittings200Ok to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetCharactersCharacterIdFittings200Ok input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) && 
                (
                    this.FittingId == input.FittingId ||
                    (this.FittingId != null &&
                    this.FittingId.Equals(input.FittingId))
                ) && 
                (
                    this.Items == input.Items ||
                    (this.Items != null &&
                    this.Items.SequenceEqual(input.Items))
                ) && 
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) && 
                (
                    this.ShipTypeId == input.ShipTypeId ||
                    (this.ShipTypeId != null &&
                    this.ShipTypeId.Equals(input.ShipTypeId))
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
                if (this.Description != null)
                    hashCode = hashCode * 59 + this.Description.GetHashCode();
                if (this.FittingId != null)
                    hashCode = hashCode * 59 + this.FittingId.GetHashCode();
                if (this.Items != null)
                    hashCode = hashCode * 59 + this.Items.GetHashCode();
                if (this.Name != null)
                    hashCode = hashCode * 59 + this.Name.GetHashCode();
                if (this.ShipTypeId != null)
                    hashCode = hashCode * 59 + this.ShipTypeId.GetHashCode();
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