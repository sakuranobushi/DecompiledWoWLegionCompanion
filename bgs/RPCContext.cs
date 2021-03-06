using bnet.protocol;
using System;
using System.Runtime.CompilerServices;

namespace bgs
{
	public class RPCContext
	{
		private bnet.protocol.Header header;

		private byte[] payload;

		private RPCContextDelegate callback;

		private bool responseReceived;

		public RPCContextDelegate Callback
		{
			get
			{
				return this.callback;
			}
			set
			{
				this.callback = value;
			}
		}

		public int Context
		{
			get;
			set;
		}

		public bnet.protocol.Header Header
		{
			get
			{
				return this.header;
			}
			set
			{
				this.header = value;
			}
		}

		public byte[] Payload
		{
			get
			{
				return this.payload;
			}
			set
			{
				this.payload = value;
			}
		}

		public IProtoBuf Request
		{
			get;
			set;
		}

		public bool ResponseReceived
		{
			get
			{
				return this.responseReceived;
			}
			set
			{
				this.responseReceived = value;
			}
		}

		public int SystemId
		{
			get;
			set;
		}

		public RPCContext()
		{
		}
	}
}