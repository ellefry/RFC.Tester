using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Connector;
using System.Data;
using System.Configuration;
using System.IO;
using System.Collections;

namespace WorkRecord
{

    public class SAP
    {
        // .NET调用SAP(RFC)

        /// <summary>
        /// 引用登陆参数的类
        /// </summary>
        public RfcDestination GetDestination()
        {
            IDestinationConfiguration ID = new MyBackendConfig(PublicClass);
            RfcDestination prd = RfcDestinationManager.GetDestination(ID.GetParameters(ConfigurationManager.AppSettings["RfcName"].ToString()));
            return prd;

        }
        PublicClass PublicClass = new PublicClass();
        BLL bll;
        /// <summary>
        /// 获取指定服务器的连接字符串
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public string GetServerStrCon(SQLServerType st)
        {
            string strcon = PublicClass.strconZFSS;
            if (st == SQLServerType.JDNS01550)
                strcon = PublicClass.strconZFSS;
            else if (st == SQLServerType.JDNS01493)
                strcon = PublicClass.strconZFSS;
            else if (st == SQLServerType.JDNS01492)
                strcon = PublicClass.strconZFSS_WH;
            else if (st == SQLServerType.JDNS01491)
                strcon = PublicClass.strconZFSS_YT;
            else if (st == SQLServerType.JDNS01490)
                strcon = PublicClass.strconZFSS_NJ;
            return strcon;
        }
        /// <summary>
        /// 外协移库到外协库接口
        /// </summary>
        /// <param name="list">外协移库到外协库数据集</param>
        /// <param name="listLost">发送失败的ProductID+Tc_card</param>
        public void ZMM_MOVE_STGE_LOC_L(MZMM_MOVE_STGE_LOC model)
        {
            string strcon = GetServerStrCon(model.SQLServer);
            //外协移库到外协库接口
            try
            {
                RfcDestination prd = GetDestination();
                IRfcFunction company = GetFunction(PublicClass.FunctionZMM_MOVE_STGE_LOC_L, prd);   //调用函数名
                if (company != null)
                {
                    #region 接口参数赋值
                    IRfcStructure IM_HEADRET = company.GetStructure("IM_HEADRET");
                    IM_HEADRET.SetValue("PSTNG_DATE", model.IM_PSTNG_DATE);
                    IM_HEADRET.SetValue("DOC_DATE", model.IM_DOC_DATE);
                    IM_HEADRET.SetValue("PR_UNAME", model.IM_PR_UNAME);

                    IRfcTable TS_ITEM = company.GetTable("TS_ITEM");
                    TS_ITEM.Insert();
                    TS_ITEM.CurrentRow.SetValue("MATERIAL", model.MATERIAL);
                    TS_ITEM.CurrentRow.SetValue("PLANT", model.PLANT);
                    TS_ITEM.CurrentRow.SetValue("STGE_LOC", model.STGE_LOC);
                    TS_ITEM.CurrentRow.SetValue("ENTRY_QNT", model.ENTRY_QNT);
                    TS_ITEM.CurrentRow.SetValue("MOVE_STLOC", model.MOVE_STLOC);
                    TS_ITEM.CurrentRow.SetValue("ZSKUNO", model.ZSKUNO);
                    TS_ITEM.CurrentRow.SetValue("ZEXIDV2", model.ZEXIDV2);
                    if (PublicClass.IsTest.Equals("0"))
                        company.Invoke(prd);   //执行函数

                    //获取返回值
                    IRfcStructure EX_HEADRET = company.GetStructure("EX_HEADRET");
                    model.EX_MAT_DOC = EX_HEADRET.GetValue("MAT_DOC").ToString().Trim();
                    model.EX_DOC_YEAR = EX_HEADRET.GetValue("DOC_YEAR").ToString().Trim();
                    model.EX_PSTNG_DATE = EX_HEADRET.GetValue("PSTNG_DATE").ToString().Trim();
                    model.EX_DOC_DATE = EX_HEADRET.GetValue("DOC_DATE").ToString().Trim();
                    model.EX_PR_UNAME = EX_HEADRET.GetValue("PR_UNAME").ToString().Trim();

                    IRfcTable RETURN = company.GetTable("RETURN");
                    model.TYPE = RETURN.CurrentRow.GetValue("TYPE").ToString().Trim();
                    model.ID = RETURN.CurrentRow.GetValue("ID").ToString().Trim();
                    model.NUMBER = RETURN.CurrentRow.GetValue("NUMBER").ToString().Trim();
                    model.MESSAGE = RETURN.CurrentRow.GetValue("MESSAGE").ToString().Trim();
                    model.LOG_NO = RETURN.CurrentRow.GetValue("LOG_NO").ToString().Trim();
                    model.LOG_MSG_NO = RETURN.CurrentRow.GetValue("LOG_MSG_NO").ToString().Trim();
                    model.MESSAGE_V1 = RETURN.CurrentRow.GetValue("MESSAGE_V1").ToString().Trim();
                    model.MESSAGE_V2 = RETURN.CurrentRow.GetValue("MESSAGE_V2").ToString().Trim();
                    model.MESSAGE_V3 = RETURN.CurrentRow.GetValue("MESSAGE_V3").ToString().Trim();
                    model.MESSAGE_V4 = RETURN.CurrentRow.GetValue("MESSAGE_V4").ToString().Trim();
                    model.PARAMETER = RETURN.CurrentRow.GetValue("PARAMETER").ToString().Trim();
                    model.ROW = RETURN.CurrentRow.GetValue("ROW").ToString().Trim();
                    model.FIELD = RETURN.CurrentRow.GetValue("FIELD").ToString().Trim();
                    model.SYSTEM = RETURN.CurrentRow.GetValue("SYSTEM").ToString().Trim();
                    #endregion

                    //如果发送成功
                    if (!string.IsNullOrEmpty(model.TYPE) && PublicClass.CheckType(model.TYPE))
                    {
                        bll = new BLL();
                        model.IsOutPut = "1";
                        if (model.SQLServer == SQLServerType.JDNS01550)
                            bll.UpdateOutPut(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01493)
                            bll.UpdateOutPut_HX(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01492)
                            bll.UpdateOutPut_WH(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01491)
                            bll.UpdateOutPut_YT(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01490)
                            bll.UpdateOutPut_NJ(model.MID);
                    }
                    //写日志
                    StringBuilder log = new StringBuilder();
                    log.Append(model.SQLServer);
                    log.Append("外协移库到外协库接口:");
                    log.Append(" [Tc_card]:" + model.Tc_card + ",");
                    log.Append(" [ProductId]:" + model.ProductId + ",");
                    log.Append(" [TYPE]:" + model.TYPE + ",");
                    log.Append(" [MESSAGE]:" + model.MESSAGE + ",");
                    log.Append(" [MESSAGE_V1]:" + model.MESSAGE_V1 + ",");
                    log.Append(" [MESSAGE_V2]:" + model.MESSAGE_V2 + ",");
                    log.Append(" [MESSAGE_V3]:" + model.MESSAGE_V3 + ",");
                    log.Append(" [MESSAGE_V4]:" + model.MESSAGE_V4 + ",");
                    log.Append(" [ID]:" + model.MID + " ");
                    tblMCToSAPLog modellog = new tblMCToSAPLog();
                    modellog.Type = "M";
                    modellog.MessageType = model.TYPE;
                    modellog.LogContent = log.ToString();
                    PublicClass.WriteLog(modellog, strcon);
                }
                prd = null;
            }
            catch (Exception ex)
            {
                tblMCToSAPLog mod = new tblMCToSAPLog();
                mod.Type = "S";
                mod.MessageType = MessageType.Interface;
                mod.LogContent = "方法ZMM_MOVE_STGE_LOC_L();  " + ex.Message;
                PublicClass.WriteLog(mod, strcon);
            }
        }

        /// <summary>
        /// 外协收货移库接口
        /// </summary>
        /// <param name="list">外协收货移库数据集</param>
        /// <param name="listLost">发送失败的ProductID+Tc_card</param>
        public void ZMM_MOVE_STGE_LOC_F(MZMM_MOVE_STGE_LOC model)
        {
            string strcon = GetServerStrCon(model.SQLServer);
            //外协收货移库接口
            try
            {
                RfcDestination prd = GetDestination();
                IRfcFunction company = GetFunction(PublicClass.FunctionZMM_MOVE_STGE_LOC_F, prd);   //调用函数名
                if (company != null)
                {
                    #region 接口参数赋值
                    IRfcStructure IM_HEADRET = company.GetStructure("IM_HEADRET");
                    IM_HEADRET.SetValue("PSTNG_DATE", model.IM_PSTNG_DATE);
                    IM_HEADRET.SetValue("DOC_DATE", model.IM_DOC_DATE);
                    IM_HEADRET.SetValue("PR_UNAME", model.IM_PR_UNAME);

                    IRfcTable TS_ITEM = company.GetTable("TS_ITEM");
                    TS_ITEM.Insert();
                    TS_ITEM.CurrentRow.SetValue("MATERIAL", model.MATERIAL);
                    TS_ITEM.CurrentRow.SetValue("PLANT", model.PLANT);
                    TS_ITEM.CurrentRow.SetValue("STGE_LOC", model.STGE_LOC);
                    TS_ITEM.CurrentRow.SetValue("ENTRY_QNT", model.ENTRY_QNT);
                    TS_ITEM.CurrentRow.SetValue("MOVE_STLOC", model.MOVE_STLOC);
                    TS_ITEM.CurrentRow.SetValue("ZSKUNO", model.ZSKUNO);
                    TS_ITEM.CurrentRow.SetValue("ZEXIDV2", model.ZEXIDV2);
                    if (PublicClass.IsTest.Equals("0"))
                        company.Invoke(prd);   //执行函数

                    //获取返回值
                    IRfcStructure EX_HEADRET = company.GetStructure("EX_HEADRET");
                    model.EX_MAT_DOC = EX_HEADRET.GetValue("MAT_DOC").ToString().Trim();
                    model.EX_DOC_YEAR = EX_HEADRET.GetValue("DOC_YEAR").ToString().Trim();
                    model.EX_PSTNG_DATE = EX_HEADRET.GetValue("PSTNG_DATE").ToString().Trim();
                    model.EX_DOC_DATE = EX_HEADRET.GetValue("DOC_DATE").ToString().Trim();
                    model.EX_PR_UNAME = EX_HEADRET.GetValue("PR_UNAME").ToString().Trim();

                    IRfcTable RETURN = company.GetTable("RETURN");
                    model.TYPE = RETURN.CurrentRow.GetValue("TYPE").ToString().Trim();
                    model.ID = RETURN.CurrentRow.GetValue("ID").ToString().Trim();
                    model.NUMBER = RETURN.CurrentRow.GetValue("NUMBER").ToString().Trim();
                    model.MESSAGE = RETURN.CurrentRow.GetValue("MESSAGE").ToString().Trim();
                    model.LOG_NO = RETURN.CurrentRow.GetValue("LOG_NO").ToString().Trim();
                    model.LOG_MSG_NO = RETURN.CurrentRow.GetValue("LOG_MSG_NO").ToString().Trim();
                    model.MESSAGE_V1 = RETURN.CurrentRow.GetValue("MESSAGE_V1").ToString().Trim();
                    model.MESSAGE_V2 = RETURN.CurrentRow.GetValue("MESSAGE_V2").ToString().Trim();
                    model.MESSAGE_V3 = RETURN.CurrentRow.GetValue("MESSAGE_V3").ToString().Trim();
                    model.MESSAGE_V4 = RETURN.CurrentRow.GetValue("MESSAGE_V4").ToString().Trim();
                    model.PARAMETER = RETURN.CurrentRow.GetValue("PARAMETER").ToString().Trim();
                    model.ROW = RETURN.CurrentRow.GetValue("ROW").ToString().Trim();
                    model.FIELD = RETURN.CurrentRow.GetValue("FIELD").ToString().Trim();
                    model.SYSTEM = RETURN.CurrentRow.GetValue("SYSTEM").ToString().Trim();
                    #endregion

                    //如果发送成功
                    if (!string.IsNullOrEmpty(model.TYPE) && PublicClass.CheckType(model.TYPE))
                    {
                        bll = new BLL();
                        model.IsOutPut = "1";
                        if (model.SQLServer == SQLServerType.JDNS01550)
                            bll.UpdateOutPut(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01493)
                            bll.UpdateOutPut_HX(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01492)
                            bll.UpdateOutPut_WH(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01491)
                            bll.UpdateOutPut_YT(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01490)
                            bll.UpdateOutPut_NJ(model.MID);
                    }
                    //写日志
                    StringBuilder log = new StringBuilder();
                    log.Append(model.SQLServer);
                    log.Append("外协收货移库接口:     ");
                    log.Append(" [Tc_card]:" + model.Tc_card + ",");
                    log.Append(" [ProductId]:" + model.ProductId + ",");
                    log.Append(" [TYPE]:" + model.TYPE + ",");
                    log.Append(" [MESSAGE]:" + model.MESSAGE + ",");
                    log.Append(" [MESSAGE_V1]:" + model.MESSAGE_V1 + ",");
                    log.Append(" [MESSAGE_V2]:" + model.MESSAGE_V2 + ",");
                    log.Append(" [MESSAGE_V3]:" + model.MESSAGE_V3 + ",");
                    log.Append(" [MESSAGE_V4]:" + model.MESSAGE_V4 + ",");
                    log.Append(" [ID]:" + model.MID + " ");
                    tblMCToSAPLog modellog = new tblMCToSAPLog();
                    modellog.Type = "M";
                    modellog.MessageType = model.TYPE;
                    modellog.LogContent = log.ToString();
                    PublicClass.WriteLog(modellog, strcon);
                }
                prd = null;
            }
            catch (Exception ex)
            {
                tblMCToSAPLog mod = new tblMCToSAPLog();
                mod.Type = "S";
                mod.MessageType = MessageType.Interface;
                mod.LogContent = "方法ZMM_MOVE_STGE_LOC_F();  " + ex.Message;
                PublicClass.WriteLog(mod, strcon);
            }
        }

        /// <summary>
        /// 车间扫描毛胚移库接口
        /// </summary>
        /// <param name="list">车间扫描毛胚移库数据集</param>
        /// <param name="listLost">发送失败的ProductID+Tc_card</param>
        public void ZMM_MAN_CISS_MAT(MZMM_MAN_CISS_MAT model)
        {
            //车间扫描毛胚移库接口
            string strcon = GetServerStrCon(model.SQLServer);
            try
            {
                RfcDestination prd = GetDestination();
                IRfcFunction company = GetFunction(PublicClass.FunctionZMM_MAN_CISS_MAT, prd);   //调用函数名
                if (company != null)
                {
                    #region 接口参数赋值
                    IRfcStructure IM_HEADRET = company.GetStructure("IM_HEADRET");
                    IM_HEADRET.SetValue("PSTNG_DATE", model.IM_PSTNG_DATE);
                    IM_HEADRET.SetValue("DOC_DATE", model.IM_DOC_DATE);
                    IM_HEADRET.SetValue("PR_UNAME", model.IM_PR_UNAME);
                    //IM_HEADRET.SetValue("IM_GERM", model.IM_GERM);

                    IRfcTable TS_ITEM = company.GetTable("TS_ITEM");
                    TS_ITEM.Insert();
                    TS_ITEM.CurrentRow.SetValue("MATERIAL", model.MATERIAL);
                    TS_ITEM.CurrentRow.SetValue("PLANT", model.PLANT);
                    TS_ITEM.CurrentRow.SetValue("STGE_LOC", model.STGE_LOC);
                    TS_ITEM.CurrentRow.SetValue("ZSKUNO", model.ZSKUNO);
                    TS_ITEM.CurrentRow.SetValue("ZEXIDV2", model.ZEXIDV2);
                    TS_ITEM.CurrentRow.SetValue("ENTRY_QNT", model.ENTRY_QNT);

                    TS_ITEM.CurrentRow.SetValue("MOVE_STLOC", model.MOVE_STLOC);
                    if (PublicClass.IsTest.Equals("0"))
                        company.Invoke(prd);   //执行函数

                    //获取返回值
                    IRfcStructure EX_HEADRET = company.GetStructure("EX_HEADRET");
                    model.EX_MAT_DOC = EX_HEADRET.GetValue("MAT_DOC").ToString().Trim();
                    model.EX_DOC_YEAR = EX_HEADRET.GetValue("DOC_YEAR").ToString().Trim();
                    model.EX_PSTNG_DATE = EX_HEADRET.GetValue("PSTNG_DATE").ToString().Trim();
                    model.EX_DOC_DATE = EX_HEADRET.GetValue("DOC_DATE").ToString().Trim();
                    model.EX_PR_UNAME = EX_HEADRET.GetValue("PR_UNAME").ToString().Trim();

                    IRfcTable RETURN = company.GetTable("RETURN");

                    model.TYPE = RETURN.CurrentRow.GetValue("TYPE").ToString().Trim();
                    model.ID = RETURN.CurrentRow.GetValue("ID").ToString().Trim();
                    model.NUMBER = RETURN.CurrentRow.GetValue("NUMBER").ToString().Trim();
                    model.MESSAGE = RETURN.CurrentRow.GetValue("MESSAGE").ToString().Trim();
                    model.LOG_NO = RETURN.CurrentRow.GetValue("LOG_NO").ToString().Trim();
                    model.LOG_MSG_NO = RETURN.CurrentRow.GetValue("LOG_MSG_NO").ToString().Trim();
                    model.MESSAGE_V1 = RETURN.CurrentRow.GetValue("MESSAGE_V1").ToString().Trim();
                    model.MESSAGE_V2 = RETURN.CurrentRow.GetValue("MESSAGE_V2").ToString().Trim();
                    model.MESSAGE_V3 = RETURN.CurrentRow.GetValue("MESSAGE_V3").ToString().Trim();
                    model.MESSAGE_V4 = RETURN.CurrentRow.GetValue("MESSAGE_V4").ToString().Trim();
                    model.PARAMETER = RETURN.CurrentRow.GetValue("PARAMETER").ToString().Trim();
                    model.ROW = RETURN.CurrentRow.GetValue("ROW").ToString().Trim();
                    model.FIELD = RETURN.CurrentRow.GetValue("FIELD").ToString().Trim();
                    model.SYSTEM = RETURN.CurrentRow.GetValue("SYSTEM").ToString().Trim();

                    #endregion

                    //如果发送成功
                    if (!string.IsNullOrEmpty(model.TYPE) && PublicClass.CheckType(model.TYPE))
                    {
                        bll = new BLL();
                        model.IsOutPut = "1";
                        if (model.SQLServer == SQLServerType.JDNS01550)
                            bll.UpdateOutPutMAT(model.MATERIAL, model.Tc_card);
                        else if (model.SQLServer == SQLServerType.JDNS01493)
                            bll.UpdateOutPutMAT_HX(model.MATERIAL, model.Tc_card);
                        else if (model.SQLServer == SQLServerType.JDNS01492)
                            bll.UpdateOutPutMAT_WH(model.MATERIAL, model.Tc_card);
                        else if (model.SQLServer == SQLServerType.JDNS01491)
                            bll.UpdateOutPutMAT_YT(model.MATERIAL, model.Tc_card);
                        else if (model.SQLServer == SQLServerType.JDNS01490)
                            bll.UpdateOutPutMAT_NJ(model.MATERIAL, model.Tc_card);
                    }
                    //写日志
                    StringBuilder log = new StringBuilder();
                    log.Append(model.SQLServer);
                    log.Append("车间扫描毛胚移库接口:");
                    log.Append(" [Tc_card]:" + model.Tc_card + ",");
                    log.Append(" [ProductId]:" + model.ProductId + ",");
                    log.Append(" [TYPE]:" + model.TYPE + ",");
                    log.Append(" [MESSAGE]:" + model.MESSAGE + ",");
                    log.Append(" [MESSAGE_V1]:" + model.MESSAGE_V1 + ",");
                    log.Append(" [MESSAGE_V2]:" + model.MESSAGE_V2 + ",");
                    log.Append(" [MESSAGE_V3]:" + model.MESSAGE_V3 + ",");
                    log.Append(" [MESSAGE_V4]:" + model.MESSAGE_V4 + ",");
                    log.Append(" [ID]:" + model.MID + " ");
                    tblMCToSAPLog modellog = new tblMCToSAPLog();
                    modellog.Type = "M";
                    modellog.MessageType = model.TYPE;
                    modellog.LogContent = log.ToString();
                    PublicClass.WriteLog(modellog, strcon);
                }
                prd = null;
            }
            catch (Exception ex)
            {
                tblMCToSAPLog mod = new tblMCToSAPLog();
                mod.Type = "S";
                mod.MessageType = MessageType.Interface;
                mod.LogContent = "方法ZMM_MAN_CISS_MAT();  " + ex.Message;
                PublicClass.WriteLog(mod, strcon);
            }

        }


        /// <summary>
        /// 报废
        /// </summary>
        /// <param name="list">报废数据集</param>
        /// <param name="listLost">发送失败的ProductID+Tc_card</param>
        public void ZMM_DEAL_SCRAP(MZMM_DEAL_SCRAP model)
        {
            string strcon = GetServerStrCon(model.SQLServer);
            //报废
            try
            {
                RfcDestination prd = GetDestination();
                IRfcFunction company = GetFunction(PublicClass.FunctionZMM_DEAL_SCRAP, prd);   //调用函数名
                if (company != null)
                {
                    #region 接口参数赋值
                    company.SetValue("I_REF_DOC_NO", model.IM_REF_DOC_NO);
                    company.SetValue("I_PSTNG_DATE", model.IM_PSTNG_DATE);
                    company.SetValue("I_DOC_DATE", model.IM_DOC_DATE);
                    company.SetValue("I_MATERIAL", model.IM_MATERIAL);
                    company.SetValue("I_VORNR", model.IM_VORNR);

                    IRfcTable TS_ITEM = company.GetTable("TS_ITEM");
                    TS_ITEM.Insert();
                    TS_ITEM.CurrentRow.SetValue("MATERIAL", model.MATERIAL);
                    TS_ITEM.CurrentRow.SetValue("PLANT", model.PLANT);
                    TS_ITEM.CurrentRow.SetValue("STGE_LOC", model.STGE_LOC);
                    TS_ITEM.CurrentRow.SetValue("ENTRY_QNT", model.ENTRY_QNT);
                    TS_ITEM.CurrentRow.SetValue("ZSKUNO", model.ZSKUNO);
                    TS_ITEM.CurrentRow.SetValue("MOVE_REAS", model.MOVE_REAS);
                    if (PublicClass.IsTest.Equals("0"))
                        company.Invoke(prd);   //执行函数

                    //获取返回值
                    IRfcStructure EX_HEADRET = company.GetStructure("EX_HEADRET");
                    model.EX_MAT_DOC = EX_HEADRET.GetValue("MAT_DOC").ToString().Trim();
                    model.EX_DOC_YEAR = EX_HEADRET.GetValue("DOC_YEAR").ToString().Trim();
                    model.EX_REF_DOC_NO = EX_HEADRET.GetValue("REF_DOC_NO").ToString().Trim();
                    model.EX_PSTNG_DATE = EX_HEADRET.GetValue("PSTNG_DATE").ToString().Trim();
                    model.EX_DOC_DATE = EX_HEADRET.GetValue("DOC_DATE").ToString().Trim();
                    model.EX_PR_UNAME = EX_HEADRET.GetValue("PR_UNAME").ToString().Trim();
                    model.EX_DOC_DATE = EX_HEADRET.GetValue("DOC_DATE").ToString().Trim();
                    model.EX_PR_UNAME = EX_HEADRET.GetValue("PR_UNAME").ToString().Trim();

                    IRfcTable RETURN = company.GetTable("RETURN");

                    model.TYPE = RETURN.CurrentRow.GetValue("TYPE").ToString().Trim();
                    model.ID = RETURN.CurrentRow.GetValue("ID").ToString().Trim();
                    model.NUMBER = RETURN.CurrentRow.GetValue("NUMBER").ToString().Trim();
                    model.MESSAGE = RETURN.CurrentRow.GetValue("MESSAGE").ToString().Trim();
                    model.LOG_NO = RETURN.CurrentRow.GetValue("LOG_NO").ToString().Trim();
                    model.LOG_MSG_NO = RETURN.CurrentRow.GetValue("LOG_MSG_NO").ToString().Trim();
                    model.MESSAGE_V1 = RETURN.CurrentRow.GetValue("MESSAGE_V1").ToString().Trim();
                    model.MESSAGE_V2 = RETURN.CurrentRow.GetValue("MESSAGE_V2").ToString().Trim();
                    model.MESSAGE_V3 = RETURN.CurrentRow.GetValue("MESSAGE_V3").ToString().Trim();
                    model.MESSAGE_V4 = RETURN.CurrentRow.GetValue("MESSAGE_V4").ToString().Trim();
                    model.PARAMETER = RETURN.CurrentRow.GetValue("PARAMETER").ToString().Trim();
                    model.ROW = RETURN.CurrentRow.GetValue("ROW").ToString().Trim();
                    model.FIELD = RETURN.CurrentRow.GetValue("FIELD").ToString().Trim();
                    model.SYSTEM = RETURN.CurrentRow.GetValue("SYSTEM").ToString().Trim();

                    #endregion

                    //如果发送成功
                    if (!string.IsNullOrEmpty(model.TYPE) && PublicClass.CheckType(model.TYPE))
                    {
                        bll = new BLL();
                        model.IsOutPut = "1";
                        if (model.SQLServer == SQLServerType.JDNS01550)
                            bll.UpdateOutPutbf(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01493)
                            bll.UpdateOutPut_HX(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01492)
                            bll.UpdateOutPut_WH(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01491)
                            bll.UpdateOutPut_YT(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01490)
                            bll.UpdateOutPut_NJ(model.MID);
                    }
                    //写日志
                    StringBuilder log = new StringBuilder();
                    log.Append(model.SQLServer);
                    log.Append("报废:                ");
                    log.Append(" [Tc_card]:" + model.Tc_card + ",");
                    log.Append(" [ProductId]:" + model.ProductId + ",");
                    log.Append(" [TYPE]:" + model.TYPE + ",");
                    log.Append(" [MESSAGE]:" + model.MESSAGE + ",");
                    log.Append(" [MESSAGE_V1]:" + model.MESSAGE_V1 + ",");
                    log.Append(" [MESSAGE_V2]:" + model.MESSAGE_V2 + ",");
                    log.Append(" [MESSAGE_V3]:" + model.MESSAGE_V3 + ",");
                    log.Append(" [MESSAGE_V4]:" + model.MESSAGE_V4 + ",");
                    log.Append(" [ID]:" + model.MID + " ");
                    tblMCToSAPLog modellog = new tblMCToSAPLog();
                    modellog.Type = "M";
                    modellog.MessageType = model.TYPE;
                    modellog.LogContent = log.ToString();
                    PublicClass.WriteLog(modellog, strcon);
                }
                prd = null;
            }
            catch (Exception ex)
            {
                tblMCToSAPLog mod = new tblMCToSAPLog();
                mod.Type = "S";
                mod.MessageType = MessageType.Program;
                mod.LogContent = "方法ZMM_DEAL_SCRAP();  " + ex.Message;
                PublicClass.WriteLog(mod, strcon);
            }
        }
        /// <summary>
        /// 小零件报废
        /// </summary>
        /// <param name="list">报废数据集</param>
        /// <param name="listLost">发送失败的ProductID+Tc_card</param>
        public void ZMM_DEAL_SCRAP1(MZMM_DEAL_SCRAP model)
        {
            string strcon = GetServerStrCon(model.SQLServer);
            //报废
            try
            {
                RfcDestination prd = GetDestination();
                IRfcFunction company = GetFunction(PublicClass.FunctionZMM_DEAL_SCRAP1, prd);   //调用函数名

                //IRfcFunction company = GetFunction("BAPI_GOODSMVT_CREATE", prd);   //调用函数名
                if (company != null)
                {
                    #region 接口参数赋值
                    #region New
                    //IRfcStructure GOODSMVT_HEADER = company.GetStructure("GOODSMVT_HEADER");
                    //GOODSMVT_HEADER.SetValue("PSTNG_DATE", model.IM_PSTNG_DATE);
                    //GOODSMVT_HEADER.SetValue("DOC_DATE", model.IM_DOC_DATE);
                    //GOODSMVT_HEADER.SetValue("REF_DOC_NO", model.IM_REF_DOC_NO);
                    //GOODSMVT_HEADER.SetValue("PR_UNAME", "CCC");
                    //GOODSMVT_HEADER.SetValue("VER_GR_GI_SLIPX","X");
                    //GOODSMVT_HEADER.SetValue("HEADER_TXT", model.IM_MATERIAL);
                    //if (PublicClass.IsTest.Equals("0"))
                    //    company.Invoke(prd);   //执行函数

                    //IRfcTable GOODSMVT_ITEM = company.GetTable("GOODSMVT_ITEM");
                    //GOODSMVT_ITEM.Insert();
                    //GOODSMVT_ITEM.CurrentRow.SetValue("MATERIAL", model.MATERIAL);
                    //GOODSMVT_ITEM.CurrentRow.SetValue("PLANT", model.PLANT);
                    //GOODSMVT_ITEM.CurrentRow.SetValue("STGE_LOC", model.STGE_LOC);
                    //GOODSMVT_ITEM.CurrentRow.SetValue("MOVE_TYPE", "Z01");
                    //GOODSMVT_ITEM.CurrentRow.SetValue("ENTRY_QNT", model.ENTRY_QNT);
                    //GOODSMVT_ITEM.CurrentRow.SetValue("ENTRY_UOM", "1");
                    //GOODSMVT_ITEM.CurrentRow.SetValue("COSTCENTER", "A");
                    //GOODSMVT_ITEM.CurrentRow.SetValue("MOVE_REAS", model.MOVE_REAS);
                    //if (PublicClass.IsTest.Equals("0"))
                    //    company.Invoke(prd);   //执行函数



                    //IRfcTable RETURN = company.GetTable("RETURN");

                    //model.TYPE = RETURN.CurrentRow.GetValue("TYPE").ToString().Trim();
                    //model.ID = RETURN.CurrentRow.GetValue("ID").ToString().Trim();
                    //model.NUMBER = RETURN.CurrentRow.GetValue("NUMBER").ToString().Trim();
                    //model.MESSAGE = RETURN.CurrentRow.GetValue("MESSAGE").ToString().Trim();
                    //model.LOG_NO = RETURN.CurrentRow.GetValue("LOG_NO").ToString().Trim();
                    //model.LOG_MSG_NO = RETURN.CurrentRow.GetValue("LOG_MSG_NO").ToString().Trim();
                    //model.MESSAGE_V1 = RETURN.CurrentRow.GetValue("MESSAGE_V1").ToString().Trim();
                    //model.MESSAGE_V2 = RETURN.CurrentRow.GetValue("MESSAGE_V2").ToString().Trim();
                    //model.MESSAGE_V3 = RETURN.CurrentRow.GetValue("MESSAGE_V3").ToString().Trim();
                    //model.MESSAGE_V4 = RETURN.CurrentRow.GetValue("MESSAGE_V4").ToString().Trim();
                    //model.PARAMETER = RETURN.CurrentRow.GetValue("PARAMETER").ToString().Trim();
                    //model.ROW = RETURN.CurrentRow.GetValue("ROW").ToString().Trim();
                    //model.FIELD = RETURN.CurrentRow.GetValue("FIELD").ToString().Trim();
                    //model.SYSTEM = RETURN.CurrentRow.GetValue("SYSTEM").ToString().Trim();
                    #endregion



                    company.SetValue("I_REF_DOC_NO", model.IM_REF_DOC_NO);
                    company.SetValue("I_PSTNG_DATE", model.IM_PSTNG_DATE);
                    company.SetValue("I_DOC_DATE", model.IM_DOC_DATE);
                    company.SetValue("I_MATERIAL", model.IM_MATERIAL);
                    company.SetValue("I_VORNR", model.IM_VORNR);

                   // IRfcTable IM_HEADRET = company.GetTable("IM_HEADRET");
                 //   IM_HEADRET.SetValue("PLANT", model.PLANT);
                    //IM_HEADRET.SetValue("MATERIAL", model.IM_MATERIAL);
                    //IM_HEADRET.SetValue("BACKFL_QUANT", model.IM_BACKFL_QUANT);
                    //IM_HEADRET.SetValue("SKU_NUM", model.IM_SKU_NUM);
                    //IM_HEADRET.SetValue("Z_GZKH", model.IM_Z_GZKH);
                    //IM_HEADRET.SetValue("PROD_VER", model.IM_PROD_VER);
                    //IM_HEADRET.SetValue("PSTNG_DATE", model.IM_PSTNG_DATE);
                    //IM_HEADRET.SetValue("OPR_NUM", model.IM_OPR_NUM);


                    //  company.SetValue("MOVE_TYPE", "Z01");
                    IRfcTable TS_ITEM = company.GetTable("TS_ITEM");
                    TS_ITEM.Insert();
                    TS_ITEM.CurrentRow.SetValue("MATERIAL", model.MATERIAL);
                    TS_ITEM.CurrentRow.SetValue("PLANT", model.PLANT);
                    TS_ITEM.CurrentRow.SetValue("STGE_LOC", model.STGE_LOC);
                    // TS_ITEM.CurrentRow.SetValue("MOVE_TYPE", "Z01");
                    TS_ITEM.CurrentRow.SetValue("ENTRY_QNT", model.ENTRY_QNT);
                    TS_ITEM.CurrentRow.SetValue("ZSKUNO", model.ZSKUNO);
                    TS_ITEM.CurrentRow.SetValue("MOVE_REAS", model.MOVE_REAS);
                    if (PublicClass.IsTest.Equals("0"))
                        company.Invoke(prd);   //执行函数

                    //获取返回值
                    IRfcStructure EX_HEADRET = company.GetStructure("EX_HEADRET");
                    model.EX_MAT_DOC = EX_HEADRET.GetValue("MAT_DOC").ToString().Trim();
                    model.EX_DOC_YEAR = EX_HEADRET.GetValue("DOC_YEAR").ToString().Trim();
                    model.EX_REF_DOC_NO = EX_HEADRET.GetValue("REF_DOC_NO").ToString().Trim();
                    model.EX_PSTNG_DATE = EX_HEADRET.GetValue("PSTNG_DATE").ToString().Trim();
                    model.EX_DOC_DATE = EX_HEADRET.GetValue("DOC_DATE").ToString().Trim();
                    model.EX_PR_UNAME = EX_HEADRET.GetValue("PR_UNAME").ToString().Trim();
                    model.EX_DOC_DATE = EX_HEADRET.GetValue("DOC_DATE").ToString().Trim();
                    model.EX_PR_UNAME = EX_HEADRET.GetValue("PR_UNAME").ToString().Trim();

                    IRfcTable RETURN = company.GetTable("RETURN");

                    model.TYPE = RETURN.CurrentRow.GetValue("TYPE").ToString().Trim();
                    model.ID = RETURN.CurrentRow.GetValue("ID").ToString().Trim();
                    model.NUMBER = RETURN.CurrentRow.GetValue("NUMBER").ToString().Trim();
                    model.MESSAGE = RETURN.CurrentRow.GetValue("MESSAGE").ToString().Trim();
                    model.LOG_NO = RETURN.CurrentRow.GetValue("LOG_NO").ToString().Trim();
                    model.LOG_MSG_NO = RETURN.CurrentRow.GetValue("LOG_MSG_NO").ToString().Trim();
                    model.MESSAGE_V1 = RETURN.CurrentRow.GetValue("MESSAGE_V1").ToString().Trim();
                    model.MESSAGE_V2 = RETURN.CurrentRow.GetValue("MESSAGE_V2").ToString().Trim();
                    model.MESSAGE_V3 = RETURN.CurrentRow.GetValue("MESSAGE_V3").ToString().Trim();
                    model.MESSAGE_V4 = RETURN.CurrentRow.GetValue("MESSAGE_V4").ToString().Trim();
                    model.PARAMETER = RETURN.CurrentRow.GetValue("PARAMETER").ToString().Trim();
                    model.ROW = RETURN.CurrentRow.GetValue("ROW").ToString().Trim();
                    model.FIELD = RETURN.CurrentRow.GetValue("FIELD").ToString().Trim();
                    model.SYSTEM = RETURN.CurrentRow.GetValue("SYSTEM").ToString().Trim();

                    #endregion

                    //如果发送成功
                    if (!string.IsNullOrEmpty(model.TYPE) && PublicClass.CheckType(model.TYPE))
                    {
                        bll = new BLL();
                        model.IsOutPut = "1";
                        if (model.SQLServer == SQLServerType.JDNS01550)
                            bll.UpdateOutPutbf(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01493)
                            bll.UpdateOutPut_HX(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01492)
                            bll.UpdateOutPut_WH(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01491)
                            bll.UpdateOutPut_YT(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01490)
                            bll.UpdateOutPut_NJ(model.MID);
                    }
                    //写日志
                    StringBuilder log = new StringBuilder();
                    log.Append(model.SQLServer);
                    log.Append("报废:                ");
                    log.Append(" [Tc_card]:" + model.Tc_card + ",");
                    log.Append(" [ProductId]:" + model.ProductId + ",");
                    log.Append(" [TYPE]:" + model.TYPE + ",");
                    log.Append(" [MESSAGE]:" + model.MESSAGE + ",");
                    log.Append(" [MESSAGE_V1]:" + model.MESSAGE_V1 + ",");
                    log.Append(" [MESSAGE_V2]:" + model.MESSAGE_V2 + ",");
                    log.Append(" [MESSAGE_V3]:" + model.MESSAGE_V3 + ",");
                    log.Append(" [MESSAGE_V4]:" + model.MESSAGE_V4 + ",");
                    log.Append(" [ID]:" + model.MID + " ");
                    tblMCToSAPLog modellog = new tblMCToSAPLog();
                    modellog.Type = "M";
                    modellog.MessageType = model.TYPE;
                    modellog.LogContent = log.ToString();
                    PublicClass.WriteLog(modellog, strcon);
                }
                prd = null;
            }
            catch (Exception ex)
            {
                tblMCToSAPLog mod = new tblMCToSAPLog();
                mod.Type = "S";
                mod.MessageType = MessageType.Program;
                mod.LogContent = "方法ZMM_DEAL_SCRAP();  " + ex.Message;
                PublicClass.WriteLog(mod, strcon);
            }
        }
        /// <summary>
        /// 完工
        /// </summary>
        /// <param name="list">完工数据集</param>
        /// <param name="listLost">发送失败的ProductID+Tc_card</param>
        public void ZPP_REPMANCONF1_CREATE_MTS(MZPP_REPMANCONF1_CREATE_MTS model)
        {
            string strcon = GetServerStrCon(model.SQLServer);
            //完工
            try
            {
                RfcDestination prd = GetDestination();
                IRfcFunction company = GetFunction(PublicClass.FunctionZPP_REPMANCONF1_CREATE_MTS, prd);   //调用函数名
                if (company != null)
                {
                    #region 接口参数赋值
                    IRfcStructure IM_HEADRET = company.GetStructure("IM_HEADRET");
                    IM_HEADRET.SetValue("PLANT", model.IM_PLANT);
                    IM_HEADRET.SetValue("MATERIAL", model.IM_MATERIAL);
                    IM_HEADRET.SetValue("BACKFL_QUANT", model.IM_BACKFL_QUANT);
                    IM_HEADRET.SetValue("SKU_NUM", model.IM_SKU_NUM);
                    IM_HEADRET.SetValue("Z_GZKH", model.IM_Z_GZKH);
                    IM_HEADRET.SetValue("PROD_VER", model.IM_PROD_VER);
                    IM_HEADRET.SetValue("PSTNG_DATE", model.IM_PSTNG_DATE);
                    IM_HEADRET.SetValue("OPR_NUM", model.IM_OPR_NUM);
                    if (PublicClass.IsTest.Equals("0"))
                        company.Invoke(prd);   //执行函数

                    //获取返回值
                    IRfcStructure EX_HEADRET = company.GetStructure("EX_HEADRET");
                    model.EX_MAT_DOC = EX_HEADRET.GetValue("MAT_DOC").ToString().Trim();
                    model.EX_DOC_YEAR = EX_HEADRET.GetValue("DOC_YEAR").ToString().Trim();
                    model.EX_PSTNG_DATE = EX_HEADRET.GetValue("PSTNG_DATE").ToString().Trim();
                    model.EX_DOC_DATE = EX_HEADRET.GetValue("DOC_DATE").ToString().Trim();
                    model.EX_PR_UNAME = EX_HEADRET.GetValue("PR_UNAME").ToString().Trim();

                    IRfcStructure EX_RETURN = company.GetStructure("EX_RETURN");
                    model.MSG_TYP = EX_RETURN.GetValue("MSG_TYP").ToString().Trim();
                    model.WERKS = EX_RETURN.GetValue("WERKS").ToString().Trim();
                    model.MATNR = EX_RETURN.GetValue("MATNR").ToString().Trim();
                    model.MAKTX = EX_RETURN.GetValue("MAKTX").ToString().Trim();
                    model.Z_ERFMG = EX_RETURN.GetValue("Z_ERFMG").ToString().Trim();
                    model.Z_SKU = EX_RETURN.GetValue("Z_SKU").ToString().Trim();
                    model.VERID = EX_RETURN.GetValue("VERID").ToString().Trim();
                    model.Z_VORNR = EX_RETURN.GetValue("Z_VORNR").ToString().Trim();
                    //model.Z_PROD_LINE = EX_RETURN.GetValue("Z_PROD_LINE").ToString().Trim();
                    model.Z_DATUM = EX_RETURN.GetValue("Z_DATUM").ToString().Trim();
                    model.MSG_TXT1 = EX_RETURN.GetValue("MSG_TXT1").ToString().Trim();
                    model.MSG_TXT2 = EX_RETURN.GetValue("MSG_TXT2").ToString().Trim();
                    model.MSG_TXT3 = EX_RETURN.GetValue("MSG_TXT3").ToString().Trim();
                    #endregion

                    //如果发送成功
                    if (!string.IsNullOrEmpty(model.MSG_TYP)
                        && (PublicClass.CheckType(model.MSG_TYP) || PublicClass.CheckMsg(model.MSG_TXT1)))
                    {
                        bll = new BLL();
                        model.IsOutPut = "1";

                        if (model.SQLServer == SQLServerType.JDNS01550)
                            bll.UpdateOutPutwg(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01493)
                            bll.UpdateOutPut_HX(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01492)
                            bll.UpdateOutPut_WH(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01491)
                            bll.UpdateOutPut_YT(model.MID);
                        else if (model.SQLServer == SQLServerType.JDNS01490)
                            bll.UpdateOutPut_NJ(model.MID);

                    }
                    //写日志
                    StringBuilder log = new StringBuilder();
                    log.Append(model.SQLServer);
                    log.Append("完工:                ");
                    log.Append(" [Tc_card]:" + model.Tc_card + ",");
                    log.Append(" [ProductId]:" + model.ProductId + ",");
                    log.Append(" [MSG_TYP]:" + model.MSG_TYP + ",");
                    log.Append(" [MSG_TXT1]:" + model.MSG_TXT1 + ",");
                    log.Append(" [MSG_TXT2]:" + model.MSG_TXT2 + ",");
                    log.Append(" [MSG_TXT3]:" + model.MSG_TXT3 + ",");
                    log.Append(" [ID]:" + model.MID + " ");
                    tblMCToSAPLog modellog = new tblMCToSAPLog();
                    modellog.Type = "M";
                    modellog.MessageType = model.MSG_TYP;
                    modellog.LogContent = log.ToString();
                    PublicClass.WriteLog(modellog, strcon);
                }
                prd = null;
            }
            catch (Exception ex)
            {
                tblMCToSAPLog mod = new tblMCToSAPLog();
                mod.Type = "S";
                mod.MessageType = MessageType.Program;
                mod.LogContent = "方法ZPP_REPMANCONF1_CREATE_MTS();  " + ex.Message;
                PublicClass.WriteLog(mod, strcon);
            }
        }
        /// <summary>
        /// 在线扫描数据接口
        /// </summary>
        /// <param name="model"></param>
        public void ZPP_ONLINE_SCAN_DATA(MZPP_ONLINE_SCAN_DATA model)
        {
            string strcon = GetServerStrCon(model.SQLServer);
            //在线扫描数据接口
            try
            {
                RfcDestination prd = GetDestination();
                IRfcFunction company = GetFunction(PublicClass.FunctionZPP_ONLINE_SCAN_DATA, prd);   //调用函数名
                if (company != null)
                {
                    #region 接口参数赋值
                    IRfcTable IM_HEADRET = company.GetTable("IM_HEADRET");
                    IM_HEADRET.Insert();
                    IM_HEADRET.CurrentRow.SetValue("MATERIAL", model.MATERIAL);
                    IM_HEADRET.CurrentRow.SetValue("PLANT", model.PLANT);
                    IM_HEADRET.CurrentRow.SetValue("STGE_LOC", model.STGE_LOC);
                    IM_HEADRET.CurrentRow.SetValue("SKU_NUM", model.SKU_NUM);
                    IM_HEADRET.CurrentRow.SetValue("PROD_LINE", model.PROD_LINE);
                    IM_HEADRET.CurrentRow.SetValue("ENTRY_QNT", model.ENTRY_QNT);
                    IM_HEADRET.CurrentRow.SetValue("PSTNG_DATE", Convert.ToDateTime(model.PSTNG_DATE));
                    if (PublicClass.IsTest.Equals("0"))
                        company.Invoke(prd);   //执行函数

                    //获取返回值
                    IRfcTable EX_RETURN = company.GetTable("EX_RETURN");
                    model.TYPE = EX_RETURN.CurrentRow.GetValue("TYPE").ToString().Trim();
                    model.EX_PLANT = EX_RETURN.CurrentRow.GetValue("PLANT").ToString().Trim();
                    model.EX_MATERIAL = EX_RETURN.CurrentRow.GetValue("MATERIAL").ToString().Trim();
                    model.EX_SKU_NUM = EX_RETURN.CurrentRow.GetValue("SKU_NUM").ToString().Trim();
                    model.EX_STGE_LOC = EX_RETURN.CurrentRow.GetValue("STGE_LOC").ToString().Trim();
                    model.EX_PROD_LINE = EX_RETURN.CurrentRow.GetValue("PROD_LINE").ToString().Trim();
                    model.EX_ENTRY_QNT = EX_RETURN.CurrentRow.GetValue("ENTRY_QNT").ToString().Trim();
                    model.EX_PSTNG_DATE = EX_RETURN.CurrentRow.GetValue("PSTNG_DATE").ToString().Trim();
                    model.MSG_TXT1 = EX_RETURN.CurrentRow.GetValue("MSG_TXT1").ToString().Trim();
                    model.MSG_TXT2 = EX_RETURN.CurrentRow.GetValue("MSG_TXT2").ToString().Trim();
                    model.MSG_TXT3 = EX_RETURN.CurrentRow.GetValue("MSG_TXT3").ToString().Trim();
                    #endregion

                    //如果发送成功
                    if (!string.IsNullOrEmpty(model.TYPE) && PublicClass.CheckType(model.TYPE))
                    {
                        bll = new BLL();
                        model.IsOutPut = "1";
                        if (model.SQLServer == SQLServerType.JDNS01550)
                        {
                            bll.UpdateOutPutSCAN(model.ID);
                        }
                        else if (model.SQLServer == SQLServerType.JDNS01492)
                        {
                            bll.UpdateOutPutSCAN_WH(model.ID);
                        }
                        else if (model.SQLServer == SQLServerType.JDNS01491)
                        {
                            bll.UpdateOutPutSCAN_YT(model.ID);
                        }
                        else if (model.SQLServer == SQLServerType.JDNS01490)
                        {
                            bll.UpdateOutPutSCAN_NJ(model.ID);
                        }
                    }
                    //写日志
                    StringBuilder log = new StringBuilder();
                    log.Append(model.SQLServer);
                    log.Append("在线扫描数据接口:     ");
                    log.Append(" [Tc_card]: " + model.Tc_card + ",");
                    log.Append(" [ProductId]: " + model.ProductId + ",");
                    log.Append(" [TYPE]:" + model.TYPE + ",");
                    log.Append(" [MSG_TXT1]:" + model.MSG_TXT1 + ",");
                    log.Append(" [MSG_TXT2]:" + model.MSG_TXT2 + ",");
                    log.Append(" [MSG_TXT3]:" + model.MSG_TXT3 + ",");
                    tblMCToSAPLog modellog = new tblMCToSAPLog();
                    modellog.Type = "M";
                    modellog.MessageType = model.TYPE;
                    modellog.LogContent = log.ToString();
                    PublicClass.WriteLog(modellog, strcon);
                }
                prd = null;
            }
            catch (Exception ex)
            {
                tblMCToSAPLog mod = new tblMCToSAPLog();
                mod.Type = "S";
                mod.MessageType = MessageType.Interface;
                mod.LogContent = "方法ZPP_ONLINE_SCAN_DATA();  " + ex.Message;
                PublicClass.WriteLog(mod, strcon);
            }

        }

        /// <summary>
        /// 移库数据接口
        /// </summary>
        /// <param name="model"></param>
        public void ZMMF_RFID_WS_DATA(ZMMF_RFID_WS model)
        {
            string strcon = GetServerStrCon(model.SQLServer);
            //移库数据接口
            try
            {
                RfcDestination prd = GetDestination();
                IRfcFunction company = GetFunction(PublicClass.FunctionZMMF_RFID_WS_DATA, prd);   //调用函数名
                if (company != null)
                {
                    //company.SetValue("I_REF_DOC_NO", model.IM_REF_DOC_NO);
                    //company.SetValue("I_PSTNG_DATE", model.IM_PSTNG_DATE);
                    //company.SetValue("I_DOC_DATE", model.IM_DOC_DATE);
                    //company.SetValue("I_MATERIAL", model.IM_MATERIAL);
                    //company.SetValue("I_VORNR", model.IM_VORNR);
                    #region 接口参数赋值
                    //IRfcTable IM_HEADRET = company.GetTable("TS_ITEM");
                    //IM_HEADRET.Insert();
                    company.SetValue("ZSJNO", model.ZSJNO);
                    company.SetValue("ZTYPE", model.ZTYPE);
                    company.SetValue("WERKS", model.WERKS);
                    company.SetValue("ZPLTN", model.ZPLTN);
                    company.SetValue("ZPOINT", model.ZPOINT);
                    company.SetValue("MATNR", model.MATNR);
                    company.SetValue("EXIDV", model.EXIDV);
                    company.SetValue("ZLGORT_ORI", model.ZLGORT_ORI);
                    company.SetValue("ZLGORT_TAR", model.ZLGORT_TAR);
                    company.SetValue("ERFMG", Convert.ToInt32(model.ERFMG));
                    if (PublicClass.IsTest.Equals("0"))
                        company.Invoke(prd);   //执行函数

                    //获取返回值
                    IRfcTable EX_RETURN = company.GetTable("GT_OUTPUT");
                    model.ZSJNO1 = EX_RETURN.CurrentRow.GetValue("ZSJNO").ToString().Trim();
                    model.ZTYPE1 = EX_RETURN.CurrentRow.GetValue("ZTYPE").ToString().Trim();
                    model.M_TYPE = EX_RETURN.CurrentRow.GetValue("M_TYPE").ToString().Trim();
                    model.M_MESS = EX_RETURN.CurrentRow.GetValue("M_MESS").ToString().Trim();

                    #endregion

                    //如果发送成功
                    if (!string.IsNullOrEmpty(model.M_TYPE) && model.M_TYPE.Equals("S"))
                    {
                        bll = new BLL();
                        model.IsOutPut = "1";
                        if (model.SQLServer == SQLServerType.JDNS01550)
                        {
                            bll.UpdateOutPutRFID(model.ID);
                        }
                        else if (model.SQLServer == SQLServerType.JDNS01492)
                        {
                            bll.UpdateOutPutRFID_WH(model.ID);
                        }
                        else if (model.SQLServer == SQLServerType.JDNS01491)
                        {
                            bll.UpdateOutPutRFID_YT(model.ID);
                        }
                        else if (model.SQLServer == SQLServerType.JDNS01490)
                        {
                            bll.UpdateOutPutRFID_NJ(model.ID);
                        }
                    }
                    //写日志
                    StringBuilder log = new StringBuilder();
                    log.Append(model.SQLServer);
                    log.Append("移库数据接口:     ");
                    log.Append(" [Tc_card]: " + model.ZSJNO + ",");
                    log.Append(" [ProductId]: " + model.EXIDV + ",");
                    log.Append(" [TYPE]:" + model.M_TYPE + ",");
                    log.Append(" [MSG_TXT1]:" + model.M_MESS + ",");
                    log.Append(" [MSG_TXT2]:" + model.M_MESS + ",");
                    log.Append(" [MSG_TXT3]:" + model.M_MESS + ",");
                    tblMCToSAPLog modellog = new tblMCToSAPLog();
                    modellog.Type = "M";
                    modellog.MessageType = model.M_TYPE;
                    modellog.LogContent = log.ToString();
                    PublicClass.WriteLog(modellog, strcon);
                }
                prd = null;
            }
            catch (Exception ex)
            {
                tblMCToSAPLog mod = new tblMCToSAPLog();
                mod.Type = "S";
                mod.MessageType = MessageType.Interface;
                mod.LogContent = "方法ZMMF_RFID_WS_DATA();  " + ex.Message;
                PublicClass.WriteLog(mod, strcon);
            }

        }

        public string Test(string name)
        {
            RfcDestination prd = GetDestination();
            //在线扫描数据接口
            IRfcFunction company = GetFunction(name, prd);   //调用函数名
            return company.ToString();
        }
        /// <summary>
        /// 将内表转换成DataTable
        /// </summary>
        /// <param name="rfcTable">内表名称</param>
        /// <returns>返回一个DataTable</returns>
        public DataTable ConvertToTable(IRfcTable rfcTable)
        {
            DataTable dt = new DataTable();


            //建立表结构
            for (int col = 0; col < rfcTable.ElementCount; col++)
            {
                RfcElementMetadata rfcCol = rfcTable.GetElementMetadata(col);
                string columnName = rfcCol.Name;
                dt.Columns.Add(columnName);
            }

            for (int rx = 0; rx < rfcTable.RowCount; rx++)
            {
                object[] dr = new object[rfcTable.ElementCount];
                for (int cx = 0; cx < dt.Columns.Count; cx++)
                {
                    dr[cx] = rfcTable[rx][dt.Columns[cx].ColumnName].GetValue();
                }
                dt.Rows.Add(dr);
            }
            return dt;

        }
        /// <summary>
        /// 根据函数名获取函数
        /// </summary>
        /// <param name="FunctionName">函数名</param>
        /// <param name="prd">RfcDestination</param>
        /// <returns></returns>
        public IRfcFunction GetFunction(string FunctionName, RfcDestination prd)
        {
            try
            {
                RfcRepository repo = prd.Repository;
                IRfcFunction company = repo.CreateFunction(FunctionName);   //调用函数名
                return company;
            }
            catch (ArgumentNullException ex)
            {
                //空异常，通常情况由RfcRepository repo = prd.Repository;这段代码触发
                //测试时，服务器连接参数出现问题会触发
                tblMCToSAPLog mod = new tblMCToSAPLog();
                mod.Type = "S";
                mod.MessageType = MessageType.Interface;
                mod.LogContent = "方法GetFunction();  " + ex;
                PublicClass.WriteLog(mod);
                return null;
            }
            catch (Exception ex)
            {
                //调用的函数不存在时可以触发该异常
                tblMCToSAPLog mod = new tblMCToSAPLog();
                mod.Type = "S";
                mod.MessageType = MessageType.Interface;
                mod.LogContent = "方法GetFunction();  " + ex;
                PublicClass.WriteLog(mod);
                return null;
            }

        }

        /// <summary>
        /// 设置登陆参数
        /// </summary>
        public class MyBackendConfig : IDestinationConfiguration
        {
            private PublicClass PublicClass = null;
            public MyBackendConfig(PublicClass PublicClass)
            {
                this.PublicClass = PublicClass;
            }
            /// <summary>
            /// 获取连接参数
            /// </summary>
            /// <param name="destinationName">RfcName</param>
            /// <returns></returns>
            public RfcConfigParameters GetParameters(String destinationName)
            {

                if (ConfigurationManager.AppSettings["RfcName"].ToString().Equals(destinationName))
                {

                    RfcConfigParameters parms = new RfcConfigParameters();

                    parms.Add(RfcConfigParameters.Name, ConfigurationManager.AppSettings["RfcName"].ToString());

                    parms.Add(RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["RfcAppServerHost"].ToString());   //SAP主机IP

                    parms.Add(RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["RfcSystemNumber"].ToString());  //SAP实例

                    parms.Add(RfcConfigParameters.User, ConfigurationManager.AppSettings["RfcUser"].ToString());  //用户名

                    parms.Add(RfcConfigParameters.Password, PublicClass.RfcPassword);  //密码
                    //parms.Add(RfcConfigParameters.Password, ConfigurationManager.AppSettings["RfcPassword"].ToString());  //密码

                    parms.Add(RfcConfigParameters.Client, ConfigurationManager.AppSettings["RfcClient"].ToString());  // Client

                    parms.Add(RfcConfigParameters.Language, ConfigurationManager.AppSettings["RfcLanguage"].ToString());  //登陆语言

                    parms.Add(RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings["RfcPoolSize"].ToString());

                    parms.Add(RfcConfigParameters.MaxPoolSize, ConfigurationManager.AppSettings["RfcMaxPoolSize"].ToString());

                    parms.Add(RfcConfigParameters.IdleTimeout, ConfigurationManager.AppSettings["RfcIdleTimeout"].ToString());

                    return parms;

                }

                else
                    return null;

            }

            public bool ChangeEventsSupported()
            {

                return false;

            }

            public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        }

    }
}
