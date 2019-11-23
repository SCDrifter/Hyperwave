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
    /// pin object
    /// </summary>
    [DataContract]
    public partial class GetCharactersCharacterIdPlanetsPlanetIdPin :  IEquatable<GetCharactersCharacterIdPlanetsPlanetIdPin>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCharactersCharacterIdPlanetsPlanetIdPin" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetCharactersCharacterIdPlanetsPlanetIdPin() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCharactersCharacterIdPlanetsPlanetIdPin" /> class.
        /// </summary>
        /// <param name="Contents">contents array.</param>
        /// <param name="ExpiryTime">expiry_time string.</param>
        /// <param name="ExtractorDetails">ExtractorDetails.</param>
        /// <param name="FactoryDetails">FactoryDetails.</param>
        /// <param name="InstallTime">install_time string.</param>
        /// <param name="LastCycleStart">last_cycle_start string.</param>
        /// <param name="Latitude">latitude number (required).</param>
        /// <param name="Longitude">longitude number (required).</param>
        /// <param name="PinId">pin_id integer (required).</param>
        /// <param name="SchematicId">schematic_id integer.</param>
        /// <param name="TypeId">type_id integer (required).</param>
        public GetCharactersCharacterIdPlanetsPlanetIdPin(List<GetCharactersCharacterIdPlanetsPlanetIdContent> Contents = default(List<GetCharactersCharacterIdPlanetsPlanetIdContent>), DateTime? ExpiryTime = default(DateTime?), GetCharactersCharacterIdPlanetsPlanetIdExtractorDetails ExtractorDetails = default(GetCharactersCharacterIdPlanetsPlanetIdExtractorDetails), GetCharactersCharacterIdPlanetsPlanetIdFactoryDetails FactoryDetails = default(GetCharactersCharacterIdPlanetsPlanetIdFactoryDetails), DateTime? InstallTime = default(DateTime?), DateTime? LastCycleStart = default(DateTime?), float? Latitude = default(float?), float? Longitude = default(float?), long? PinId = default(long?), int? SchematicId = default(int?), int? TypeId = default(int?))
        {
            // to ensure "Latitude" is required (not null)
            if (Latitude == null)
            {
                throw new InvalidDataException("Latitude is a required property for GetCharactersCharacterIdPlanetsPlanetIdPin and cannot be null");
            }
            else
            {
                this.Latitude = Latitude;
            }
            // to ensure "Longitude" is required (not null)
            if (Longitude == null)
            {
                throw new InvalidDataException("Longitude is a required property for GetCharactersCharacterIdPlanetsPlanetIdPin and cannot be null");
            }
            else
            {
                this.Longitude = Longitude;
            }
            // to ensure "PinId" is required (not null)
            if (PinId == null)
            {
                throw new InvalidDataException("PinId is a required property for GetCharactersCharacterIdPlanetsPlanetIdPin and cannot be null");
            }
            else
            {
                this.PinId = PinId;
            }
            // to ensure "TypeId" is required (not null)
            if (TypeId == null)
            {
                throw new InvalidDataException("TypeId is a required property for GetCharactersCharacterIdPlanetsPlanetIdPin and cannot be null");
            }
            else
            {
                this.TypeId = TypeId;
            }
            this.Contents = Contents;
            this.ExpiryTime = ExpiryTime;
            this.ExtractorDetails = ExtractorDetails;
            this.FactoryDetails = FactoryDetails;
            this.InstallTime = InstallTime;
            this.LastCycleStart = LastCycleStart;
            this.SchematicId = SchematicId;
        }
        
        /// <summary>
        /// contents array
        /// </summary>
        /// <value>contents array</value>
        [DataMember(Name="contents", EmitDefaultValue=false)]
        public List<GetCharactersCharacterIdPlanetsPlanetIdContent> Contents { get; set; }

        /// <summary>
        /// expiry_time string
        /// </summary>
        /// <value>expiry_time string</value>
        [DataMember(Name="expiry_time", EmitDefaultValue=false)]
        public DateTime? ExpiryTime { get; set; }

        /// <summary>
        /// Gets or Sets ExtractorDetails
        /// </summary>
        [DataMember(Name="extractor_details", EmitDefaultValue=false)]
        public GetCharactersCharacterIdPlanetsPlanetIdExtractorDetails ExtractorDetails { get; set; }

        /// <summary>
        /// Gets or Sets FactoryDetails
        /// </summary>
        [DataMember(Name="factory_details", EmitDefaultValue=false)]
        public GetCharactersCharacterIdPlanetsPlanetIdFactoryDetails FactoryDetails { get; set; }

        /// <summary>
        /// install_time string
        /// </summary>
        /// <value>install_time string</value>
        [DataMember(Name="install_time", EmitDefaultValue=false)]
        public DateTime? InstallTime { get; set; }

        /// <summary>
        /// last_cycle_start string
        /// </summary>
        /// <value>last_cycle_start string</value>
        [DataMember(Name="last_cycle_start", EmitDefaultValue=false)]
        public DateTime? LastCycleStart { get; set; }

        /// <summary>
        /// latitude number
        /// </summary>
        /// <value>latitude number</value>
        [DataMember(Name="latitude", EmitDefaultValue=false)]
        public float? Latitude { get; set; }

        /// <summary>
        /// longitude number
        /// </summary>
        /// <value>longitude number</value>
        [DataMember(Name="longitude", EmitDefaultValue=false)]
        public float? Longitude { get; set; }

        /// <summary>
        /// pin_id integer
        /// </summary>
        /// <value>pin_id integer</value>
        [DataMember(Name="pin_id", EmitDefaultValue=false)]
        public long? PinId { get; set; }

        /// <summary>
        /// schematic_id integer
        /// </summary>
        /// <value>schematic_id integer</value>
        [DataMember(Name="schematic_id", EmitDefaultValue=false)]
        public int? SchematicId { get; set; }

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
            sb.Append("class GetCharactersCharacterIdPlanetsPlanetIdPin {\n");
            sb.Append("  Contents: ").Append(Contents).Append("\n");
            sb.Append("  ExpiryTime: ").Append(ExpiryTime).Append("\n");
            sb.Append("  ExtractorDetails: ").Append(ExtractorDetails).Append("\n");
            sb.Append("  FactoryDetails: ").Append(FactoryDetails).Append("\n");
            sb.Append("  InstallTime: ").Append(InstallTime).Append("\n");
            sb.Append("  LastCycleStart: ").Append(LastCycleStart).Append("\n");
            sb.Append("  Latitude: ").Append(Latitude).Append("\n");
            sb.Append("  Longitude: ").Append(Longitude).Append("\n");
            sb.Append("  PinId: ").Append(PinId).Append("\n");
            sb.Append("  SchematicId: ").Append(SchematicId).Append("\n");
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
            return this.Equals(input as GetCharactersCharacterIdPlanetsPlanetIdPin);
        }

        /// <summary>
        /// Returns true if GetCharactersCharacterIdPlanetsPlanetIdPin instances are equal
        /// </summary>
        /// <param name="input">Instance of GetCharactersCharacterIdPlanetsPlanetIdPin to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetCharactersCharacterIdPlanetsPlanetIdPin input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Contents == input.Contents ||
                    (this.Contents != null &&
                    this.Contents.SequenceEqual(input.Contents))
                ) && 
                (
                    this.ExpiryTime == input.ExpiryTime ||
                    (this.ExpiryTime != null &&
                    this.ExpiryTime.Equals(input.ExpiryTime))
                ) && 
                (
                    this.ExtractorDetails == input.ExtractorDetails ||
                    (this.ExtractorDetails != null &&
                    this.ExtractorDetails.Equals(input.ExtractorDetails))
                ) && 
                (
                    this.FactoryDetails == input.FactoryDetails ||
                    (this.FactoryDetails != null &&
                    this.FactoryDetails.Equals(input.FactoryDetails))
                ) && 
                (
                    this.InstallTime == input.InstallTime ||
                    (this.InstallTime != null &&
                    this.InstallTime.Equals(input.InstallTime))
                ) && 
                (
                    this.LastCycleStart == input.LastCycleStart ||
                    (this.LastCycleStart != null &&
                    this.LastCycleStart.Equals(input.LastCycleStart))
                ) && 
                (
                    this.Latitude == input.Latitude ||
                    (this.Latitude != null &&
                    this.Latitude.Equals(input.Latitude))
                ) && 
                (
                    this.Longitude == input.Longitude ||
                    (this.Longitude != null &&
                    this.Longitude.Equals(input.Longitude))
                ) && 
                (
                    this.PinId == input.PinId ||
                    (this.PinId != null &&
                    this.PinId.Equals(input.PinId))
                ) && 
                (
                    this.SchematicId == input.SchematicId ||
                    (this.SchematicId != null &&
                    this.SchematicId.Equals(input.SchematicId))
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
                if (this.Contents != null)
                    hashCode = hashCode * 59 + this.Contents.GetHashCode();
                if (this.ExpiryTime != null)
                    hashCode = hashCode * 59 + this.ExpiryTime.GetHashCode();
                if (this.ExtractorDetails != null)
                    hashCode = hashCode * 59 + this.ExtractorDetails.GetHashCode();
                if (this.FactoryDetails != null)
                    hashCode = hashCode * 59 + this.FactoryDetails.GetHashCode();
                if (this.InstallTime != null)
                    hashCode = hashCode * 59 + this.InstallTime.GetHashCode();
                if (this.LastCycleStart != null)
                    hashCode = hashCode * 59 + this.LastCycleStart.GetHashCode();
                if (this.Latitude != null)
                    hashCode = hashCode * 59 + this.Latitude.GetHashCode();
                if (this.Longitude != null)
                    hashCode = hashCode * 59 + this.Longitude.GetHashCode();
                if (this.PinId != null)
                    hashCode = hashCode * 59 + this.PinId.GetHashCode();
                if (this.SchematicId != null)
                    hashCode = hashCode * 59 + this.SchematicId.GetHashCode();
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
