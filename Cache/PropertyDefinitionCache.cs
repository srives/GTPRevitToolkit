using Gtpx.ModelSync.DataModel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PropertyDefinition = Gtpx.ModelSync.DataModel.Models.PropertyDefinition;

namespace Gtpx.ModelSync.CAD.Cache
{
    public class PropertyDefinitionCache
    {
        private FileStream _cacheFileStream;
        private string _cacheFileName = string.Empty;
        private Dictionary<string, bool> _isIdCached;
        private readonly Dictionary<string, SortedSet<string>> templateIdToPropertyDefinitionIds;
        private bool _opened = false;

        /// <summary>
        /// Number of properties across all elements in the cache
        /// </summary>
        public int Count { get; private set; } = 0;

        public PropertyDefinitionCache()
        {
            templateIdToPropertyDefinitionIds = new Dictionary<string, SortedSet<string>>();
            _isIdCached = new Dictionary<string, bool>();
            CreateCacheFile();
        }


        ~PropertyDefinitionCache()
        {
            if (_opened)
            {
                _cacheFileStream.Close(); // Note well: closing a file after it is already closed is still safe.
            }
        }

        private void CloseCacheFile()
        {
            if (_opened)
            {
                _cacheFileStream.Close(); // Note well: closing a file after it is already closed is still safe.
            }
        }

        private bool CreateCacheFile()
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
                _cacheFileStream = new FileStream(this._cacheFileName, FileMode.Append);
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
        private string GetFileNameAndErase(string fname)
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

        public void Add(PropertyDefinition propertyDefinition, Element element)
        {
            var id = $"{propertyDefinition.Name}:{propertyDefinition.StorageDataType}:{propertyDefinition.DisplayDataType}";
            propertyDefinition.Id = id;
            if (!_isIdCached.ContainsKey(id))
            {
                var bFormatter = new BinaryFormatter();
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

        public IEnumerable<PropertyDefinition> GetPropertyDefinitions()
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
        public IEnumerable<string> GetPropertyDefinitionIds(Element element)
        {
            foreach (var item in templateIdToPropertyDefinitionIds[element.TemplateId])
            {
                yield return item;
            }
        }
    }
}
