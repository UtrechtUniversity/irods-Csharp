using System;
using System.Collections.Generic;
using System.Linq;

namespace irods_Csharp
{
    public class QueryManager
    {
        private readonly IrodsSession _session;
        private readonly Path _home;

        /// <summary>
        /// Constructor for query manager.
        /// </summary>
        /// <param name="session">Session which contains account and connection</param>
        /// <param name="home">Path to home directory</param>
        public QueryManager(IrodsSession session, string home)
        {
            _session = session;
            _home = new Path(home);
        }

        /// <summary>
        /// Perform general query with supplied conditions and select statements, casts results to supplied type.
        /// </summary>
        /// <param name="path">Path of collection where should be searched</param>
        /// <param name="select">Array of table columns which should be queried</param>
        /// <param name="conditions">Array of conditions for query</param>
        /// <param name="type">Type to which query results should be cast</param>
        /// <param name="maxRows">Maximum amount of rows to query</param>
        /// <returns>Array of objects of the supplied type</returns>
        internal object Query(string path, Column[] select, Condition[] conditions, Type type, int maxRows = 500)
        {
            InxIvalPair_PI selects = new InxIvalPair_PI(select.Length, select.Select(x => x.Id).ToArray(), Enumerable.Repeat(1, select.Length).ToArray());
            InxValPair_PI cConditions = new InxValPair_PI(conditions.Length, conditions.Select(x => x.Column.Id).ToArray(), conditions.Select(x => x.ToString()).ToArray());

            //TODO Implement keyword conditions
            KeyValPair_PI kConditions = new KeyValPair_PI(0, null, null);

            Packet<GenQueryInp_PI> query = new Packet<GenQueryInp_PI>(ApiNumberData.GEN_QUERY_AN)
            {
                MsgBody = new GenQueryInp_PI(maxRows, 0, 0, 0, kConditions, selects, cConditions)
            };

            _session.SendPacket(query);

            Packet<GenQueryOut_PI> queryResult = _session.ReceivePacket<GenQueryOut_PI>();

            return queryResult.MsgBody.Parse(type, _session, _home, new Path(path));
        }

        /// <summary>
        /// Queries collections based on name.
        /// </summary>
        /// <param name="path">Path of collection where should be searched</param>
        /// <param name="name">Name of collection which should be matched</param>
        /// <param name="strict">Whether or not the collection name should match exactly</param>
        /// <param name="maxRows">Maximum amount of rows to query</param>
        /// <returns>Array of matching collections</returns>
        public Collection[] QueryCollection(string path, string name, bool strict = false, int maxRows = 500)
        {
            Condition[] conditions = strict
                ? new[]
                {
                    new Condition(QueryModels.COLL_NAME, "=", (_home + path) + "/" + name)
                }
                : new[]
                {
                    new Condition(QueryModels.COLL_NAME, "like", (_home + path) + "%"),
                    new Condition(QueryModels.COLL_NAME, "like", "%" + name + "%")

                };

            return (Collection[])Query(path, QueryModels.Collection(), conditions, typeof(Collection));
        }

        /// <summary>
        /// Queries objects based on name
        /// </summary>
        /// <param name="name">Name of Data Object which should be matched</param>
        /// <param name="path">Path of collection where should be searched</param>
        /// <param name="collectionId">Id of parent collection, will be queried if left unspecified</param>
        /// <param name="maxRows">Maximum amount of rows to query</param>
        /// <returns>Array of matching objects</returns>
        public DataObj[] QueryObj(string name, string path, int collectionId = -1, int maxRows = 500)
        {
            collectionId = CollectionCheck(collectionId, path);

            List<Condition> conditions = new List<Condition>
            {
                new Condition(QueryModels.DATA_NAME, "like", "%" + name + "%"),
                new Condition(QueryModels.D_COLL_ID, "=", collectionId.ToString())
            };

            return (DataObj[])Query(path, QueryModels.DataObject(), conditions.ToArray(), typeof(DataObj));
        }

        /// <summary>
        /// Queries collections based on metadata
        /// </summary>
        /// <param name="path">Path of collection where should be searched</param>
        /// <param name="metaName">Name of meta triplet to search</param>
        /// <param name="metaValue">Value of meta triplet to search</param>
        /// <param name="metaUnits">Units of meta triplet to search</param>
        /// <param name="maxRows">Maximum amount of rows to query</param>
        /// <returns>Array of matching collections</returns>
        public Collection[] MQueryCollection(string path, string metaName = "", string metaValue = "", int metaUnits = -1, int maxRows = 500)
        {
            List<Condition> conditions = new List<Condition>()
            {
                new Condition(QueryModels.COLL_NAME, "like", _home + "%"),
                new Condition(QueryModels.COLL_NAME, "like", "%"+path+"/%")
            };

            if (metaName != "") conditions.Add(new Condition(QueryModels.COL_META_COLL_ATTR_NAME, "=", metaName));
            if (metaValue != "") conditions.Add(new Condition(QueryModels.COL_META_COLL_ATTR_VALUE, "=", metaValue));
            if (metaUnits >= 0) conditions.Add(new Condition(QueryModels.COL_META_COLL_ATTR_UNITS, "=", metaUnits.ToString()));

            return (Collection[])Query(path, QueryModels.Collection(), conditions.ToArray(), typeof(Collection));
        }

        /// <summary>
        /// Queries data objects based on metadata
        /// </summary>
        /// <param name="path">Path of collection where should be searched</param>
        /// <param name="metaName">Name of meta triplet to search</param>
        /// <param name="metaValue">Value of meta triplet to search</param>
        /// <param name="metaUnits">Units of meta triplet to search</param>
        /// <param name="collectionId">Id of parent collection, will be queried if left unspecified</param>
        /// <param name="maxRows">Maximum amount of rows to query</param>
        /// <returns>Array of matching objects</returns>
        public DataObj[] MQueryObj(string path, string metaName = "", string metaValue = "", int metaUnits = -1, int collectionId = -1, int maxRows = 500)
        {
            collectionId = CollectionCheck(collectionId, path);

            List<Condition> conditions = new List<Condition>();
            if (path != "") conditions.Add(new Condition(QueryModels.D_COLL_ID, "=", collectionId.ToString()));

            if (metaName != "") conditions.Add(new Condition(QueryModels.COL_META_DATA_ATTR_NAME, "=", metaName));
            if (metaValue != "") conditions.Add(new Condition(QueryModels.COL_META_DATA_ATTR_VALUE, "=", metaValue));
            if (metaUnits >= 0) conditions.Add(new Condition(QueryModels.COL_META_DATA_ATTR_UNITS, "=", metaUnits.ToString()));

            return (DataObj[])Query(path, QueryModels.DataObject(), conditions.ToArray(), typeof(DataObj));
        }

        /// <summary>
        /// Finds id corresponding to path, if this isn't given.
        /// </summary>
        /// <param name="collectionId">Id of collection, if it negative then the id still needs to be found</param>
        /// <param name="path">Path to collection</param>
        /// <returns>Collection id</returns>
        private int CollectionCheck(int collectionId, string path)
        {
            if (collectionId < 0)
            {
                Collection[] coll = (Collection[])Query(path, QueryModels.Collection(), new[] { new Condition(QueryModels.COLL_NAME, "like", (_home + path)) }, typeof(Collection));
                collectionId = coll[0].Id;
            }
            return collectionId;
        }

        /// <summary>
        /// Returns meta tags attached to object.
        /// </summary>
        /// <param name="path">Path to parent collection of object</param>
        /// <param name="type">Type of object</param>
        /// <param name="maxRows">Maximum amount of rows to query</param>
        /// <returns>Array of metadata</returns>
        public Meta[] QueryMeta(string path, string type, int maxRows = 500)
        {
            Condition[] conditions = { };
            Column[] selects = { };
            switch (type)
            {
                case "-c":
                    conditions = new[]
                    {
                        new Condition(QueryModels.COLL_NAME, "=", _home + path)
                    };
                    selects = QueryModels.CollectionMeta();
                    break;
                case "-d":
                    Collection[] coll = (Collection[])Query(path, QueryModels.Collection(), new[]
                    {
                        new Condition(QueryModels.COLL_NAME, "=", _home + Path.First(path))
                    }, typeof(Collection));
                    int collectionId = coll[0].Id;
                    conditions = new[]
                    {
                        new Condition(QueryModels.DATA_NAME, "=", Path.Last(path)),
                        new Condition(QueryModels.D_COLL_ID, "=", collectionId.ToString())
                    };
                    selects = QueryModels.DataObjMeta();
                    break;

            }
            return (Meta[])Query(path, selects, conditions, typeof(Meta));
        }
    }
}
