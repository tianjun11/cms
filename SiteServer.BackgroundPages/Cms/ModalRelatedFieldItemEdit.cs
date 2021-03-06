﻿using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;

namespace SiteServer.BackgroundPages.Cms
{
    public class ModalRelatedFieldItemEdit : BasePageCms
    {
        public TextBox TbItemName;
        public TextBox TbItemValue;

        private int _relatedFieldId;
        private int _parentId;
        private int _level;
        private int _id;

        public static string GetOpenWindowString(int publishmentSystemId, int relatedFieldId, int parentId, int level, int id)
        {
            return LayerUtils.GetOpenScript("编辑字段项", PageUtils.GetCmsUrl(nameof(ModalRelatedFieldItemEdit), new NameValueCollection
            {
                {"PublishmentSystemID", publishmentSystemId.ToString()},
                {"RelatedFieldID", relatedFieldId.ToString()},
                {"ParentID", parentId.ToString()},
                {"Level", level.ToString()},
                {"ID", id.ToString()}
            }), 320, 260);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsForbidden) return;

            _relatedFieldId = Body.GetQueryInt("RelatedFieldID");
            _parentId = Body.GetQueryInt("ParentID");
            _level = Body.GetQueryInt("Level");
            _id = Body.GetQueryInt("ID");

            if (IsPostBack) return;

            var itemInfo = DataProvider.RelatedFieldItemDao.GetRelatedFieldItemInfo(_id);
            TbItemName.Text = itemInfo.ItemName;
            TbItemValue.Text = itemInfo.ItemValue;
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged;

            try
            {
                var itemInfo = DataProvider.RelatedFieldItemDao.GetRelatedFieldItemInfo(_id);
                itemInfo.ItemName = TbItemName.Text;
                itemInfo.ItemValue = TbItemValue.Text;
                DataProvider.RelatedFieldItemDao.Update(itemInfo);

                isChanged = true;
            }
            catch(Exception ex)
            {
                isChanged = false;
                FailMessage(ex, ex.Message);
            }

            if (isChanged)
            {
                LayerUtils.CloseAndRedirect(Page, PageRelatedFieldItem.GetRedirectUrl(PublishmentSystemId, _relatedFieldId, _parentId, _level));
            }
        }
    }
}
