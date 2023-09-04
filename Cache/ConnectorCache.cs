using Autodesk.Revit.DB;
using RTree;
using System.Collections.Generic;
using System.Linq;

namespace Gtpx.ModelSync.Export.Revit.Caches
{
    public static class ConnectorCache
    {
        private static Dictionary<ElementId, IEnumerable<Connector>> elementIdToConnectorsMap = new Dictionary<ElementId, IEnumerable<Connector>>();
        private static RTree<Connector> connectorRTree = new RTree<Connector>();
        private static double connectionTolerance = 0.5 / 12.0;
        private static double lengthTolerance = 1.0 / 32.0 / 12.0;

        public static void Reset()
        {
            elementIdToConnectorsMap = new Dictionary<ElementId, IEnumerable<Connector>>();
            connectorRTree = new RTree<Connector>();
        }

        /// <returns>true if success, false if errors</returns>
        public static bool CacheConnectors(Element element)
        {
            IEnumerable<Connector> connectors = new List<Connector>();

            // CableTray, Conduit, Duct, Pipe
            if (element is FabricationPart fabricationPart)
            {
                connectors = GetConnectors(fabricationPart.ConnectorManager);
            }
            else if (element is FamilyInstance familyInstance)
            {
                connectors = GetConnectors(familyInstance.MEPModel.ConnectorManager);
            }
            else if (element is MEPCurve mepCurve)
            {
                connectors = GetConnectors(mepCurve.ConnectorManager);
            }

            elementIdToConnectorsMap[element.Id] = connectors;
            return StoreConnectorsInRTree(connectors);
        }

        public static IEnumerable<Connector> GetConnectors(Element element)
        {
            IEnumerable<Connector> connectors = new Connector[] { };

            if (elementIdToConnectorsMap.TryGetValue(element.Id, out IEnumerable<Connector> cachedConnectors))
            {
                connectors = cachedConnectors;
            }

            return connectors;
        }

        public static IEnumerable<Element> GetNearbyElements(
            Connector connector,
            Document document)
        {
            var connectors = GetNearestConnectors(connector, connectionTolerance);
#if Revit2024
            var elementIds = connectors.Select(x => x.Owner.Id.Value)
                                       .Distinct();
#else
            var elementIds = connectors.Select(x => x.Owner.Id.IntegerValue)
                                       .Distinct();
#endif
            return elementIds.Select(x => document.GetElement(new ElementId(x)));
        }

        public static bool TryGet(
           Connector connector,
           IEnumerable<Connector> otherEndConnectors,
           Element otherElement,
           out string matingPartCadId,
           out int matingPartConnIndex,
           out double minDistance)
        {
            matingPartConnIndex = -1;
            matingPartCadId = string.Empty;
            var connectorIndex = 0;
            minDistance = -1;
            var foundMate = false;
            foreach (var otherConnector in otherEndConnectors)
            {
                if (otherConnector.Origin.DistanceTo(connector.Origin) < lengthTolerance)
                {
                    var distance = otherConnector.Origin.DistanceTo(connector.Origin);
                    if (minDistance == -1 || minDistance > distance)
                    {
                        minDistance = distance;
                        matingPartConnIndex = connectorIndex;
                        matingPartCadId = otherElement.UniqueId;
                        foundMate = true;
                    }
                }
                connectorIndex++;
            }
            return foundMate;
        }

        private static IEnumerable<Connector> GetNearestConnectors(
            Connector connector,
            double connectionTolerance)
        {
            var o = connector.Origin;

            var rect = new RTree.Rectangle((float)(o.X + connectionTolerance),
                                           (float)(o.Y + connectionTolerance),
                                           (float)(o.X - connectionTolerance),
                                           (float)(o.Y - connectionTolerance),
                                           (float)(o.Z + connectionTolerance),
                                           (float)(o.Z - connectionTolerance));

            return connectorRTree.Contains(rect).Where(x => x.Owner.Id != connector.Owner.Id);
        }

        private static IEnumerable<Connector> GetConnectors(ConnectorManager connectorManager)
        {
            IEnumerable<Connector> connectors = new Connector[] { };

            if (connectorManager != null &&
                connectorManager.Connectors != null &&
                !connectorManager.Connectors.IsEmpty)
            {
                connectors = connectorManager.Connectors.OfType<Connector>()
                                                        .Where(x => x.ConnectorType == ConnectorType.Curve || x.ConnectorType == ConnectorType.End)
                                                        .OrderBy(x => x.Id)
                                                        .ToArray();
            }

            return connectors;
        }

        /// <returns>true if success, false if errors</returns>
        private static bool StoreConnectorsInRTree(IEnumerable<Connector> connectors)
        {
            var success = true;
            foreach (var connector in connectors)
            {
                try
                {
                    var o = connector.Origin; // Warning: this can throw an exception
                    var rect = new RTree.Rectangle((float)o.X,
                                                   (float)o.Y,
                                                   (float)o.X,
                                                   (float)o.Y,
                                                   (float)o.Z,
                                                   (float)o.Z);
                    connectorRTree.Add(rect, connector);
                }
                catch
                {
                    // AutoDesk can throw an error when accessing an invalid connector.Origin
                    success = false;
                }
            }
            return success;
        }
    }
}
