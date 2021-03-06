﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GDC.PH.AIDE.BusinessLayer.DataLayer;
/// <summary>
/// By Jhunell G. Barcenas
/// </summary>
namespace GDC.PH.AIDE.BusinessLayer
{
    public class clsResourcePlannerFactory
    {

        #region data Members

        clsResourcePlannerSql _dataObject = null;

        #endregion

        #region Constructor

        public clsResourcePlannerFactory()
        {
            _dataObject = new clsResourcePlannerSql();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new clsAttendance
        /// </summary>
        /// <param name="businessObject">clsAttendance object</param>
        /// <returns>true for successfully saved</returns>
 
        //public bool InsertResourcePlanner(clsResourcePlanner businessObject)
        //{
        //    if (!businessObject.IsValid)
        //    {
        //        throw new InvalidBusinessObjectException(businessObject.BrokenRulesList.ToString());
        //    }

        //    return _dataObject.InsertResourcePlanner(businessObject);
        //}

        /// <summary>
        /// Update existing clsAttendance
        /// </summary>
        /// <param name="businessObject">clsAttendance object</param>
        /// <returns>true for successfully saved</returns>
        public bool UpdateResourcePlanner(clsResourcePlanner businessObject)
        {
            if (!businessObject.IsValid)
            {
                throw new InvalidBusinessObjectException(businessObject.BrokenRulesList.ToString());
            }
            return _dataObject.UpdateResourcePlanner(businessObject);
        }

        public List<clsResourcePlanner> ViewEmpResourcePlanner(int deptID)
        {
            return _dataObject.ViewEmpResourcePlanner(deptID);
        }

        public List<clsResourcePlanner> GetStatusResourcePlanner()
        {
            return _dataObject.GetStatusResourcePlanner();
        }

        public List<clsResourcePlanner> GetResourcePlannerByEmpID(int empID, int deptID, int month, int year)
        {
            return _dataObject.GetResourcePlannerByEmpID(empID, deptID, month, year);
        }

        public List<clsResourcePlanner> GetAllEmpResourcePlanner(int deptID, int month, int year)
        {
            return _dataObject.GetAllEmpResourcePlanner(deptID, month, year);
        }

        public List<clsResourcePlanner> GetAllEmpResourcePlannerByStatus(int deptID, int month, int year, int status)
        {
            return _dataObject.GetAllEmpResourcePlannerByStatus(deptID, month, year, status);
        }

        public List<clsResourcePlanner> GetAllStatusResourcePlanner()
        {
            return _dataObject.GetAllStatusResourcePlanner();
        }

        #endregion
    }
}
