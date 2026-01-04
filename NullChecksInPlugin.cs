using System;
using Microsoft.Xrm.Sdk;
namespace NHSBSA.PAD.Helper
{
    /// <summary>
    ///   Global Function
    /// </summary>
    public static class NullChecksInPlugin
    {
        public static EntityReference GetDynamicEntityRefrenceValue(Entity entity, Entity image, string attributeName)
        {
            return (entity.Contains(attributeName)) ? entity.GetAttributeValue<EntityReference>(attributeName) :
                  (image == null ? null : (image.Contains(attributeName) ? image.GetAttributeValue<EntityReference>(attributeName) : null));
        }

        public static int GetDynamicIntValue(Entity entity, Entity image, string attributeName)
        {
            return entity != null ?
                ((entity.Contains(attributeName)) ? entity.GetAttributeValue<int>(attributeName) : (image == null ? 0 : (image.Contains(attributeName) ? image.GetAttributeValue<int>(attributeName) : 0)))
                : (image == null ? 0 : (image.Contains(attributeName) ? image.GetAttributeValue<int>(attributeName) : 0));
        }

        public static decimal GetDynamicDecimalValue(Entity entity, Entity image, string attributeName)
        {
            return (entity.Contains(attributeName)) ? entity.GetAttributeValue<decimal>(attributeName) :
                  (image == null ? 0 : (image.Contains(attributeName) ? image.GetAttributeValue<decimal>(attributeName) : 0));
        }
        public static OptionSetValue GetDynamicOptionSetValue(Entity entity, Entity image, string attributeName)
        {
            return (entity.Contains(attributeName)) ? entity.GetAttributeValue<OptionSetValue>(attributeName) :
                  (image == null ? null : (image.Contains(attributeName) ? image.GetAttributeValue<OptionSetValue>(attributeName) : null));
        }

        public static bool GetDynamicBoolValue(Entity entity, Entity image, string attributeName)
        {
            return (entity.Contains(attributeName)) ? entity.GetAttributeValue<bool>(attributeName) :
                  (image == null ? false : (image.Contains(attributeName) ? image.GetAttributeValue<bool>(attributeName) : false));
        }
        public static string GetDynamicStringValue(Entity entity, Entity image, string attributeName)
        {
            return (entity.Contains(attributeName)) ? entity.GetAttributeValue<string>(attributeName) :
                  (image == null ? null : (image.Contains(attributeName) ? image.GetAttributeValue<string>(attributeName) : null));
        }

        public static DateTime? GetDynamicDateTimeValue(Entity entity, Entity preImage, string fieldName)
        {
            if (entity.Contains(fieldName))
            {
                return entity.GetAttributeValue<DateTime>(fieldName);
            }
            else if (preImage != null && preImage.Contains(fieldName))
            {
                return preImage.GetAttributeValue<DateTime>(fieldName);
            }
            return null;
        }
    }
}