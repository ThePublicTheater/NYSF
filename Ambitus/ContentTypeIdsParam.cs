using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ambitus
{
	public class ContentTypeIdsParam : IEnumerable<int>
	{
		private int[] contentTypeIds;
		private bool excludeOnlyWebContent;

		public int this[int i]
		{
			get
			{
				return contentTypeIds[i];
			}
		}

		public int Count
		{
			get
			{
				if (contentTypeIds == null)
				{
					return 0;
				}
				return contentTypeIds.Length;
			}
		}

		public bool ExcludeOnlyWebContent
		{
			get
			{
				return excludeOnlyWebContent;
			}
		}

		public ContentTypeIdsParam()
				: this(null, null) { }

		public ContentTypeIdsParam(int contentTypeId)
				: this(new int[] {contentTypeId}, null) { }

		public ContentTypeIdsParam(int[] contentTypeIds)
				: this(contentTypeIds, null) { }

		public ContentTypeIdsParam(List<int> contentTypeIds)
				: this(contentTypeIds.ToArray(), null) { }

		public ContentTypeIdsParam(bool excludeOnlyWebContent)
				: this(null, excludeOnlyWebContent) { }

		private ContentTypeIdsParam(int[] contentTypeIds, bool? excludeOnlyWebContent)
		{
			this.contentTypeIds = contentTypeIds;
			this.excludeOnlyWebContent = excludeOnlyWebContent ?? false;
		}

		public override string ToString()
		{
			if (excludeOnlyWebContent)
			{
				return Tess.EmptyStringLiteral;
			}
			if (contentTypeIds == null)
			{
				return String.Empty;
			}
			return String.Join(",", contentTypeIds);
		}

		public IEnumerator<int> GetEnumerator()
		{
			return contentTypeIds.AsEnumerable<int>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
