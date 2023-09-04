using Gtpx.ModelSync.DataModel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PropertyDefinition = Gtpx.ModelSync.DataModel.Models.PropertyDefinition;

namespace Gtpx.ModelSync.CAD.Cache
{
    public static class PropertyDefinitionCache
    {
        private static FileStream _cacheFileStream;
        private static string _cacheFileName = string.Empty;
        private static Dictionary<string, bool> _isIdCached = new Dictionary<string, bool>();
        private static Dictionary<string, SortedSet<string>> templateIdToPropertyDefinitionIds = new Dictionary<string, SortedSet<string>>();
        private static bool _opened = false;

        /// <summary>
        /// Number of properties across all elements in the cache
        /// </summary>
        public static int Count { get; private set; } = 0;

        public static void Reset()
        {
            CloseCacheFile();
            templateIdToPropertyDefinitionIds = new Dictionary<string, SortedSet<string>>();
            _isIdCached = new Dictionary<string, bool>();
            _opened = false;
            CreateCacheFile();
        }


        private static void CloseCacheFile()
        {
            if (_opened)
            {
                _cacheFileStream.Close(); // Note well: closing a file after it is already closed is still safe.                
            }
            _opened = false;
        }

        private static bool CreateCacheFile()
        {
            var datFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                       "GTP Software Inc",
                                       @"STRATUS\CACHE",
                                       "PropertyDefinition.dat");
            CloseCacheFile();
            _cacheFileName = string.Empty;
            var directoryName = Path.GetDirectoryName(datFile);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            _cacheFileName = GetFileNameAndErase(datFile);
            try
            {
                _cacheFileStream = new FileStream(_cacheFileName, FileMode.Append);
                _opened = true;
            }
            catch
            {
                _opened = false;
            }
            return _opened;
        }

        /// <summary>
        /// Delete file. If you can't delete it, generate a new file name.
        /// </summary>
        /// <returns>Name of file to use if we cold not delete fname</returns>
        private static string GetFileNameAndErase(string fname)
        {
            var start = fname;
            var rnd = new Random();
            var i = 0;
            while (File.Exists(fname) && i++ < 20)
            {
                try
                {
                    File.Delete(fname);
                }
                catch
                {
                }
                if (File.Exists(fname))
                {
                    fname = start + "." + rnd.Next();
                }
            }
            return fname;
        }

        public static void Add(PropertyDefinition propertyDefinition, Element element)
        {            
            var id = $"{propertyDefinition.Name}:{propertyDefinition.StorageDataType}:{propertyDefinition.DisplayDataType}";
            propertyDefinition.Id = id;
            if (!_isIdCached.ContainsKey(id))
            {
                var bFormatter = new BinaryFormatter();
                if (!_opened)
                {
                    CreateCacheFile();
                }
                bFormatter.Serialize(_cacheFileStream, propertyDefinition);
                _isIdCached[id] = true;
            }
            if (element?.TemplateId != null)
            {
                if (!templateIdToPropertyDefinitionIds.ContainsKey(element.TemplateId))
                {
                    templateIdToPropertyDefinitionIds[element.TemplateId] = new SortedSet<string>();
                }
                templateIdToPropertyDefinitionIds[element.TemplateId].Add(id);
                Count++;
            }
            return;
        }

        public static IEnumerable<PropertyDefinition> GetPropertyDefinitions()
        {
            _cacheFileStream.Close();
            using (var fileStream = new FileStream(_cacheFileName, FileMode.Open))
            {
                var bFormatter = new BinaryFormatter();
                while (fileStream.Position != fileStream.Length)
                {
                    var ret = (PropertyDefinition)bFormatter.Deserialize(fileStream);
                    yield return ret;
                }
            }
        }

        /// <summary>
        /// Get a sorted list of all the property definition ids for the given element templateId
        /// where each id in the sorted list of returned Ids are of the format:
        ///     $"{propertyDefinition.Name}:{propertyDefinition.StorageDataType}:{propertyDefinition.DisplayDataType}";
        /// </summary>
        public static IEnumerable<string> GetPropertyDefinitionIds(Element element)
        {
            foreach (var item in templateIdToPropertyDefinitionIds[element.TemplateId])
            {
                yield return item;
            }
        }
    }
}
