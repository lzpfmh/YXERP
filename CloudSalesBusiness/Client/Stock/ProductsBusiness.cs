﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;


using CloudSalesDAL;
using CloudSalesEntity;
using CloudSalesEnum;

namespace CloudSalesBusiness
{
    public class ProductsBusiness
    {

        #region 查询

        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="keyWords">关键词</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="clientID">客户端ID</param>
        /// <returns></returns>
        public List<C_Brand> GetBrandList(string keyWords, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string clientID)
        {
            var dal = new ProductsDAL();
            DataSet ds = dal.GetBrandList(keyWords, pageSize, pageIndex, ref totalCount, ref pageCount, clientID);

            List<C_Brand> list = new List<C_Brand>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                C_Brand model = new C_Brand();
                model.FillData(dr);
                model.City = CommonCache.Citys.Where(c => c.CityCode == model.CityCode).FirstOrDefault();
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 获取品牌实体
        /// </summary>
        /// <param name="brandID">传入参数</param>
        /// <returns></returns>
        public C_Brand GetBrandByBrandID(string brandID)
        {
            var dal = new ProductsDAL();
            DataTable dt = dal.GetBrandByBrandID(brandID);

            C_Brand model = new C_Brand();
            if (dt.Rows.Count > 0)
            {
                model.FillData(dt.Rows[0]);
                model.City = CommonCache.Citys.Where(c => c.CityCode == model.CityCode).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 获取单位列表
        /// </summary>
        /// <returns></returns>
        public List<C_ProductUnit> GetClientUnits(string clientid)
        {
            var dal = new ProductsDAL();
            DataTable dt = dal.GetClientUnits(clientid);

            List<C_ProductUnit> list = new List<C_ProductUnit>();
            foreach (DataRow dr in dt.Rows)
            {
                C_ProductUnit model = new C_ProductUnit();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }

        #endregion

        #region 添加

        /// <summary>
        /// 添加品牌
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="anotherName">别称</param>
        /// <param name="icoPath"></param>
        /// <param name="countryCode">国家编码</param>
        /// <param name="cityCode">城市编码</param>
        /// <param name="status">状态</param>
        /// <param name="remark">备注</param>
        /// <param name="brandStyle">风格</param>
        /// <param name="operateIP">操作IP</param>
        /// <param name="operateID">操作人</param>
        /// <param name="clientID">客户端ID</param>
        /// <returns></returns>
        public string AddBrand(string name, string anotherName, string icoPath, string countryCode, string cityCode, int status, string remark, string brandStyle, string operateIP, string operateID, string clientID)
        {
            if (!string.IsNullOrEmpty(icoPath) && icoPath != "/modules/images/default.png")
            {
                if (icoPath.IndexOf("?") > 0)
                {
                    icoPath = icoPath.Substring(0, icoPath.IndexOf("?"));
                }
                FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath(icoPath));
                icoPath = "/Content/uploadFiles/" + file.Name;
                if (file.Exists)
                {
                    file.MoveTo(HttpContext.Current.Server.MapPath(icoPath));
                }
            }
            return new ProductsDAL().AddBrand(name, anotherName, icoPath, countryCode, cityCode, status, remark, brandStyle, operateIP, operateID, clientID);
        }

        /// <summary>
        /// 添加单位
        /// </summary>
        /// <param name="unitName">单位名称</param>
        /// <param name="description">描述</param>
        /// <returns></returns>
        public string AddUnit(string unitName, string description,string operateid,string clientid)
        {
            var dal = new ProductsDAL();
            return dal.AddUnit(unitName, description, operateid, clientid);
        }

        #endregion

        #region 编辑、删除

        /// <summary>
        /// 编辑品牌状态
        /// </summary>
        /// <param name="brandID">品牌ID</param>
        /// <param name="status">状态</param>
        /// <param name="operateIP">操作IP</param>
        /// <param name="operateID">操作人</param>
        /// <returns></returns>
        public bool UpdateBrandStatus(string brandID, StatusEnum status, string operateIP, string operateID)
        {
            return CommonBusiness.Update("C_Brand", "Status", ((int)status).ToString(), " BrandID='" + brandID + "'");
        }

        /// <summary>
        /// 编辑品牌
        /// </summary>
        /// <param name="brandID">ID</param>
        /// <param name="name">名称</param>
        /// <param name="anotherName">别称</param>
        /// <param name="countryCode">国家编码</param>
        /// <param name="cityCode">城市编码</param>
        /// <param name="status">状态</param>
        /// <param name="remark">备注</param>
        /// <param name="brandStyle"风格></param>
        /// <param name="operateIP">操作IP</param>
        /// <param name="operateID">操作人</param>
        /// <returns></returns>
        public bool UpdateBrand(string brandID, string name, string anotherName, string countryCode, string cityCode, int status, string remark, string brandStyle, string operateIP, string operateID)
        {
            var dal = new ProductsDAL();
            return dal.UpdateBrand(brandID, name, anotherName, countryCode, cityCode, status, remark, brandStyle, operateIP, operateID);
        }

        /// <summary>
        /// 编辑单位
        /// </summary>
        /// <param name="unitID">单位ID</param>
        /// <param name="unitName">单位名称</param>
        /// <param name="desciption">描述</param>
        /// <param name="operateid">操作人</param>
        /// <returns></returns>
        public bool UpdateUnit(string unitID, string unitName, string desciption, string operateid)
        {
            var dal = new ProductsDAL();
            return dal.UpdateUnit(unitID, unitName, desciption);
        }

        /// <summary>
        /// 编辑单位状态
        /// </summary>
        /// <param name="unitID">单位ID</param>
        /// <param name="status">状态</param>
        /// <param name="operateIP">操作IP</param>
        /// <param name="operateID">操作人</param>
        /// <returns></returns>
        public bool UpdateUnitStatus(string unitID, StatusEnum status, string operateIP, string operateID)
        {
            var dal = new ProductsDAL();
            return dal.UpdateUnitStatus(unitID, (int)status);
        }

        #endregion
    }
}
