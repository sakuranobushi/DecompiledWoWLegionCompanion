using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Bson
{
	public class BsonWriter : JsonWriter
	{
		private readonly BsonBinaryWriter _writer;

		private BsonToken _root;

		private BsonToken _parent;

		private string _propertyName;

		public DateTimeKind DateTimeKindHandling
		{
			get
			{
				return this._writer.DateTimeKindHandling;
			}
			set
			{
				this._writer.DateTimeKindHandling = value;
			}
		}

		public BsonWriter(Stream stream)
		{
			ValidationUtils.ArgumentNotNull(stream, "stream");
			this._writer = new BsonBinaryWriter(stream);
		}

		private void AddParent(BsonToken container)
		{
			this.AddToken(container);
			this._parent = container;
		}

		internal void AddToken(BsonToken token)
		{
			if (this._parent == null)
			{
				if (token.Type != BsonType.Object && token.Type != BsonType.Array)
				{
					throw new JsonWriterException("Error writing {0} value. BSON must start with an Object or Array.".FormatWith(CultureInfo.InvariantCulture, new object[] { token.Type }));
				}
				this._parent = token;
				this._root = token;
			}
			else if (!(this._parent is BsonObject))
			{
				((BsonArray)this._parent).Add(token);
			}
			else
			{
				((BsonObject)this._parent).Add(this._propertyName, token);
				this._propertyName = null;
			}
		}

		private void AddValue(object value, BsonType type)
		{
			this.AddToken(new BsonValue(value, type));
		}

		public override void Close()
		{
			base.Close();
			if (base.CloseOutput && this._writer != null)
			{
				this._writer.Close();
			}
		}

		public override void Flush()
		{
			this._writer.Flush();
		}

		private void RemoveParent()
		{
			this._parent = this._parent.Parent;
		}

		public override void WriteComment(string text)
		{
			throw new JsonWriterException("Cannot write JSON comment as BSON.");
		}

		protected override void WriteEnd(JsonToken token)
		{
			base.WriteEnd(token);
			this.RemoveParent();
			if (base.Top == 0)
			{
				this._writer.WriteToken(this._root);
			}
		}

		public override void WriteNull()
		{
			base.WriteNull();
			this.AddValue(null, BsonType.Null);
		}

		public void WriteObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if ((int)value.Length != 12)
			{
				throw new Exception("An object id must be 12 bytes");
			}
			base.AutoComplete(JsonToken.Undefined);
			this.AddValue(value, BsonType.Oid);
		}

		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			this._propertyName = name;
		}

		public override void WriteRaw(string json)
		{
			throw new JsonWriterException("Cannot write raw JSON as BSON.");
		}

		public override void WriteRawValue(string json)
		{
			throw new JsonWriterException("Cannot write raw JSON as BSON.");
		}

		public void WriteRegex(string pattern, string options)
		{
			ValidationUtils.ArgumentNotNull(pattern, "pattern");
			base.AutoComplete(JsonToken.Undefined);
			this.AddToken(new BsonRegex(pattern, options));
		}

		public override void WriteStartArray()
		{
			base.WriteStartArray();
			this.AddParent(new BsonArray());
		}

		public override void WriteStartConstructor(string name)
		{
			throw new JsonWriterException("Cannot write JSON constructor as BSON.");
		}

		public override void WriteStartObject()
		{
			base.WriteStartObject();
			this.AddParent(new BsonObject());
		}

		public override void WriteUndefined()
		{
			base.WriteUndefined();
			this.AddValue(null, BsonType.Undefined);
		}

		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			if (value != null)
			{
				this.AddToken(new BsonString(value, true));
			}
			else
			{
				this.AddValue(null, BsonType.Null);
			}
		}

		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		public override void WriteValue(uint value)
		{
			if (value > 2147483647)
			{
				throw new JsonWriterException("Value is too large to fit in a signed 32 bit integer. BSON does not support unsigned values.");
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		public override void WriteValue(ulong value)
		{
			if (value > 9223372036854775807L)
			{
				throw new JsonWriterException("Value is too large to fit in a signed 64 bit integer. BSON does not support unsigned values.");
			}
			base.WriteValue(value);
			this.AddValue(value, BsonType.Long);
		}

		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Boolean);
		}

		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Integer);
		}

		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Number);
		}

		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Date);
		}

		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Date);
		}

		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			this.AddValue(value, BsonType.Binary);
		}

		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}

		public override void WriteValue(Uri value)
		{
			base.WriteValue(value);
			this.AddToken(new BsonString(value.ToString(), true));
		}
	}
}