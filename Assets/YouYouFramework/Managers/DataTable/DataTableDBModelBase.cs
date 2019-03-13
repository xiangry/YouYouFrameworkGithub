﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YouYouFramework
{
    /// <summary>
    /// 数据表管理基类
    /// </summary>
    public abstract class DataTableDBModelBase<T, P>
    where T : class, new()
    where P : DataTableEntityBase
    {
        protected List<P> m_List;
        protected Dictionary<int, P> m_Dic;

        public DataTableDBModelBase()
        {
            m_List = new List<P>();
            m_Dic = new Dictionary<int, P>();
        }

        #region 需要子类实现的属性或方法
        /// <summary>
        /// 数据表名
        /// </summary>
        public abstract string DataTableName { get; }

        /// <summary>
        /// 加载数据列表
        /// </summary>
        /// <param name="parse"></param>
        /// <returns></returns>
        protected abstract void LoadList(MMO_MemoryStream ms);
        #endregion

        #region 加载数据表数据 LoadData
        /// <summary>
        /// 加载数据表数据
        /// </summary>
        public void LoadData()
        {
            //1.获取数据表的buffer
            byte[] buffer = GameEntry.Resource.GetFileBuffer(string.Format("{0}/download/DataTable/{1}.bytes",GameEntry.Resource.LocalFilePath, DataTableName));

            //2.加载数据表数据
            using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
            {
                LoadList(ms);
            }

            //加载完成单个表，发布事件
            VarString dataName = VarString.Alloc(DataTableName);
            GameEntry.Event.CommonEvent.Dispatch(SysEventId.LoadOneDataTableComplete, dataName);
            dataName.Release();
        }
        #endregion

        #region GetList 获取集合
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <returns></returns>
        public List<P> GetList()
        {
            return m_List;
        }
        #endregion

        #region Get 根据编号获取实体
        /// <summary>
        /// 根据编号获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public P Get(int id)
        {
            if (m_Dic.ContainsKey(id))
            {
                return m_Dic[id];
            }
            return null;
        }
        #endregion

        public void Clear()
        {
            m_List.Clear();
            m_Dic.Clear();
        }
    }
}
