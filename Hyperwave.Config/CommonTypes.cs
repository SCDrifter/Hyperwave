using System;
using Eve.Api.Model;
namespace Hyperwave.Common
{
    public enum EntityType
    {
        Alliance = 0,
        Character = 1,
        Corporation = 2,
        Mailinglist = 3
    }

    static public class ESIConvert
    {
        public static EntityType RecipientTypeToEntityType(GetCharactersCharacterIdMailMailIdRecipient.RecipientTypeEnum type)
        {
            switch(type)
            {
                case GetCharactersCharacterIdMailMailIdRecipient.RecipientTypeEnum.Alliance:
                    return EntityType.Alliance;
                case GetCharactersCharacterIdMailMailIdRecipient.RecipientTypeEnum.Corporation:
                    return EntityType.Corporation;
                case GetCharactersCharacterIdMailMailIdRecipient.RecipientTypeEnum.Mailinglist:
                    return EntityType.Mailinglist;
                case GetCharactersCharacterIdMailMailIdRecipient.RecipientTypeEnum.Character:
                    return EntityType.Character;
                default:
                    throw new Exception("API mismatch");
            }
        }

        public static EntityType RecipientTypeToEntityType(GetCharactersCharacterIdMailRecipient.RecipientTypeEnum type)
        {
            switch (type)
            {
                case GetCharactersCharacterIdMailRecipient.RecipientTypeEnum.Alliance:
                    return EntityType.Alliance;
                case GetCharactersCharacterIdMailRecipient.RecipientTypeEnum.Corporation:
                    return EntityType.Corporation;
                case GetCharactersCharacterIdMailRecipient.RecipientTypeEnum.Mailinglist:
                    return EntityType.Mailinglist;
                case GetCharactersCharacterIdMailRecipient.RecipientTypeEnum.Character:
                    return EntityType.Character;
                default:
                    throw new Exception("API mismatch");
            }
        }

        public static PostCharactersCharacterIdMailRecipient.RecipientTypeEnum EntityTypeToRecipientType(EntityType type)
        {
            switch (type)
            {
                case EntityType.Alliance:
                    return PostCharactersCharacterIdMailRecipient.RecipientTypeEnum.Alliance;
                case EntityType.Corporation:
                    return PostCharactersCharacterIdMailRecipient.RecipientTypeEnum.Corporation;
                case EntityType.Mailinglist:
                    return PostCharactersCharacterIdMailRecipient.RecipientTypeEnum.Mailinglist;
                case EntityType.Character:
                    return PostCharactersCharacterIdMailRecipient.RecipientTypeEnum.Character;
                default:
                    throw new Exception("API mismatch");
            }
        }
    }
}