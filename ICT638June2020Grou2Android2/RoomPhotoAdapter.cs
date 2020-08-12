using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace ICT638June2020Grou2Android2
{
    class RoomPhotoAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;
        public RoomList mRoomlist;

        public RoomPhotoAdapter(RoomList roomlist)
        {
            mRoomlist = roomlist;
        }
        public override int ItemCount
        {
            get { return mRoomlist.numPhoto; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PhotoViewHolder vh = holder as PhotoViewHolder;
            vh.Image.SetImageResource(mRoomlist[position].mPhotoID);
            vh.Caption.Text = mRoomlist[position].roomDetails.Id.ToString();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RoomPhoto, parent, false);
            PhotoViewHolder vh = new PhotoViewHolder(itemView, OnClick);
            return vh;
        }
        private void OnClick(int obj)
        {
            if (ItemClick != null)
                ItemClick(this, obj);
        }
    }
}