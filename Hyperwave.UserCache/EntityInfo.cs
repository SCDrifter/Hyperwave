using System;
using Hyperwave.Common;

namespace Hyperwave.UserCache
{
    public class EntityInfo  :IComparable<EntityInfo>
    {
        public long EntityID { get; set; }
        public EntityType EntityType { get; set; }
        public string Name { get; set; }

        public string ImageUrl16
        {
            get
            {
                return ImageUrl(32);
            }
        }

        public string ImageUrl32
        {
            get
            {
                return ImageUrl(32);
            }
        }

        public string ImageUrl64
        {
            get
            {
                return ImageUrl(64);
            }
        }

        public string ImageUrl128
        {
            get
            {
                return ImageUrl(128);
            }
        }

        string ImageUrl(int size)
        {
            if (EntityID == -1)
                return string.Format("@://Images/Icons/{0}/GenericEntity.png", size);

            switch (EntityType)
            {
                case EntityType.Character:
                    return string.Format("https://imageserver.eveonline.com/Character/{0}_{1}.jpg", EntityID, size);
                case EntityType.Corporation:
                    return string.Format("https://imageserver.eveonline.com/Corporation/{0}_{1}.png", EntityID, size);
                case EntityType.Alliance:
                    return string.Format("https://imageserver.eveonline.com/Alliance/{0}_{1}.png", EntityID, size);
                case EntityType.Mailinglist:
                    return string.Format("@://Images/Icons/{0}/MailingList.png",size);
                default:
                    throw new InvalidOperationException("Unknown EntityType:" + EntityType);
            }
        }

        public event EventHandler LookupComplete;
        public event EventHandler LookupFailed;

        internal void OnFinished(bool sucessfull)
        {
            if (sucessfull && LookupComplete != null)
                LookupComplete(this, new EventArgs());
            else if (!sucessfull && LookupFailed != null)
                LookupFailed(this, new EventArgs());
        }

        public int CompareTo(EntityInfo other)
        {
            return EntityID.CompareTo(other.EntityID);
        }
    }
}
