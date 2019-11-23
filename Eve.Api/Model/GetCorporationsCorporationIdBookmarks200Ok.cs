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
    public partial class GetCorporationsCorporationIdBookmarks200Ok :  IEquatable<GetCorporationsCorporationIdBookmarks200Ok>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCorporationsCorporationIdBookmarks200Ok" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetCorporationsCorporationIdBookmarks200Ok() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCorporationsCorporationIdBookmarks200Ok" /> class.
        /// </summary>
        /// <param name="BookmarkId">bookmark_id integer (required).</param>
        /// <param name="Coordinates">Coordinates.</param>
        /// <param name="Created">created string (required).</param>
        /// <param name="CreatorId">creator_id integer (required).</param>
        /// <param name="FolderId">folder_id integer.</param>
        /// <param name="Item">Item.</param>
        /// <param name="Label">label string (required).</param>
        /// <param name="LocationId">location_id integer (required).</param>
        /// <param name="Notes">notes string (required).</param>
        public GetCorporationsCorporationIdBookmarks200Ok(int? BookmarkId = default(int?), GetCorporationsCorporationIdBookmarksCoordinates Coordinates = default(GetCorporationsCorporationIdBookmarksCoordinates), DateTime? Created = default(DateTime?), int? CreatorId = default(int?), int? FolderId = default(int?), GetCorporationsCorporationIdBookmarksItem Item = default(GetCorporationsCorporationIdBookmarksItem), string Label = default(string), int? LocationId = default(int?), string Notes = default(string))
        {
            // to ensure "BookmarkId" is required (not null)
            if (BookmarkId == null)
            {
                throw new InvalidDataException("BookmarkId is a required property for GetCorporationsCorporationIdBookmarks200Ok and cannot be null");
            }
            else
            {
                this.BookmarkId = BookmarkId;
            }
            // to ensure "Created" is required (not null)
            if (Created == null)
            {
                throw new InvalidDataException("Created is a required property for GetCorporationsCorporationIdBookmarks200Ok and cannot be null");
            }
            else
            {
                this.Created = Created;
            }
            // to ensure "CreatorId" is required (not null)
            if (CreatorId == null)
            {
                throw new InvalidDataException("CreatorId is a required property for GetCorporationsCorporationIdBookmarks200Ok and cannot be null");
            }
            else
            {
                this.CreatorId = CreatorId;
            }
            // to ensure "Label" is required (not null)
            if (Label == null)
            {
                throw new InvalidDataException("Label is a required property for GetCorporationsCorporationIdBookmarks200Ok and cannot be null");
            }
            else
            {
                this.Label = Label;
            }
            // to ensure "LocationId" is required (not null)
            if (LocationId == null)
            {
                throw new InvalidDataException("LocationId is a required property for GetCorporationsCorporationIdBookmarks200Ok and cannot be null");
            }
            else
            {
                this.LocationId = LocationId;
            }
            // to ensure "Notes" is required (not null)
            if (Notes == null)
            {
                throw new InvalidDataException("Notes is a required property for GetCorporationsCorporationIdBookmarks200Ok and cannot be null");
            }
            else
            {
                this.Notes = Notes;
            }
            this.Coordinates = Coordinates;
            this.FolderId = FolderId;
            this.Item = Item;
        }
        
        /// <summary>
        /// bookmark_id integer
        /// </summary>
        /// <value>bookmark_id integer</value>
        [DataMember(Name="bookmark_id", EmitDefaultValue=false)]
        public int? BookmarkId { get; set; }

        /// <summary>
        /// Gets or Sets Coordinates
        /// </summary>
        [DataMember(Name="coordinates", EmitDefaultValue=false)]
        public GetCorporationsCorporationIdBookmarksCoordinates Coordinates { get; set; }

        /// <summary>
        /// created string
        /// </summary>
        /// <value>created string</value>
        [DataMember(Name="created", EmitDefaultValue=false)]
        public DateTime? Created { get; set; }

        /// <summary>
        /// creator_id integer
        /// </summary>
        /// <value>creator_id integer</value>
        [DataMember(Name="creator_id", EmitDefaultValue=false)]
        public int? CreatorId { get; set; }

        /// <summary>
        /// folder_id integer
        /// </summary>
        /// <value>folder_id integer</value>
        [DataMember(Name="folder_id", EmitDefaultValue=false)]
        public int? FolderId { get; set; }

        /// <summary>
        /// Gets or Sets Item
        /// </summary>
        [DataMember(Name="item", EmitDefaultValue=false)]
        public GetCorporationsCorporationIdBookmarksItem Item { get; set; }

        /// <summary>
        /// label string
        /// </summary>
        /// <value>label string</value>
        [DataMember(Name="label", EmitDefaultValue=false)]
        public string Label { get; set; }

        /// <summary>
        /// location_id integer
        /// </summary>
        /// <value>location_id integer</value>
        [DataMember(Name="location_id", EmitDefaultValue=false)]
        public int? LocationId { get; set; }

        /// <summary>
        /// notes string
        /// </summary>
        /// <value>notes string</value>
        [DataMember(Name="notes", EmitDefaultValue=false)]
        public string Notes { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetCorporationsCorporationIdBookmarks200Ok {\n");
            sb.Append("  BookmarkId: ").Append(BookmarkId).Append("\n");
            sb.Append("  Coordinates: ").Append(Coordinates).Append("\n");
            sb.Append("  Created: ").Append(Created).Append("\n");
            sb.Append("  CreatorId: ").Append(CreatorId).Append("\n");
            sb.Append("  FolderId: ").Append(FolderId).Append("\n");
            sb.Append("  Item: ").Append(Item).Append("\n");
            sb.Append("  Label: ").Append(Label).Append("\n");
            sb.Append("  LocationId: ").Append(LocationId).Append("\n");
            sb.Append("  Notes: ").Append(Notes).Append("\n");
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
            return this.Equals(input as GetCorporationsCorporationIdBookmarks200Ok);
        }

        /// <summary>
        /// Returns true if GetCorporationsCorporationIdBookmarks200Ok instances are equal
        /// </summary>
        /// <param name="input">Instance of GetCorporationsCorporationIdBookmarks200Ok to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetCorporationsCorporationIdBookmarks200Ok input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.BookmarkId == input.BookmarkId ||
                    (this.BookmarkId != null &&
                    this.BookmarkId.Equals(input.BookmarkId))
                ) && 
                (
                    this.Coordinates == input.Coordinates ||
                    (this.Coordinates != null &&
                    this.Coordinates.Equals(input.Coordinates))
                ) && 
                (
                    this.Created == input.Created ||
                    (this.Created != null &&
                    this.Created.Equals(input.Created))
                ) && 
                (
                    this.CreatorId == input.CreatorId ||
                    (this.CreatorId != null &&
                    this.CreatorId.Equals(input.CreatorId))
                ) && 
                (
                    this.FolderId == input.FolderId ||
                    (this.FolderId != null &&
                    this.FolderId.Equals(input.FolderId))
                ) && 
                (
                    this.Item == input.Item ||
                    (this.Item != null &&
                    this.Item.Equals(input.Item))
                ) && 
                (
                    this.Label == input.Label ||
                    (this.Label != null &&
                    this.Label.Equals(input.Label))
                ) && 
                (
                    this.LocationId == input.LocationId ||
                    (this.LocationId != null &&
                    this.LocationId.Equals(input.LocationId))
                ) && 
                (
                    this.Notes == input.Notes ||
                    (this.Notes != null &&
                    this.Notes.Equals(input.Notes))
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
                if (this.BookmarkId != null)
                    hashCode = hashCode * 59 + this.BookmarkId.GetHashCode();
                if (this.Coordinates != null)
                    hashCode = hashCode * 59 + this.Coordinates.GetHashCode();
                if (this.Created != null)
                    hashCode = hashCode * 59 + this.Created.GetHashCode();
                if (this.CreatorId != null)
                    hashCode = hashCode * 59 + this.CreatorId.GetHashCode();
                if (this.FolderId != null)
                    hashCode = hashCode * 59 + this.FolderId.GetHashCode();
                if (this.Item != null)
                    hashCode = hashCode * 59 + this.Item.GetHashCode();
                if (this.Label != null)
                    hashCode = hashCode * 59 + this.Label.GetHashCode();
                if (this.LocationId != null)
                    hashCode = hashCode * 59 + this.LocationId.GetHashCode();
                if (this.Notes != null)
                    hashCode = hashCode * 59 + this.Notes.GetHashCode();
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
