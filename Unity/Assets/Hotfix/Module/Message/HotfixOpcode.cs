using ETModel;
namespace ETHotfix
{
	[Message(HotfixOpcode.C2R_Login)]
	public partial class C2R_Login : IRequest {}

	[Message(HotfixOpcode.R2C_Login)]
	public partial class R2C_Login : IResponse {}

	[Message(HotfixOpcode.C2R_Register)]
	public partial class C2R_Register : IRequest {}

	[Message(HotfixOpcode.R2C_Register)]
	public partial class R2C_Register : IResponse {}

	[Message(HotfixOpcode.C2G_LoginGate)]
	public partial class C2G_LoginGate : IRequest {}

	[Message(HotfixOpcode.G2C_LoginGate)]
	public partial class G2C_LoginGate : IResponse {}

	[Message(HotfixOpcode.G2C_TestHotfixMessage)]
	public partial class G2C_TestHotfixMessage : IMessage {}

	[Message(HotfixOpcode.C2M_TestActorRequest)]
	public partial class C2M_TestActorRequest : IActorLocationRequest {}

	[Message(HotfixOpcode.M2C_TestActorResponse)]
	public partial class M2C_TestActorResponse : IActorLocationResponse {}

	[Message(HotfixOpcode.PlayerInfo)]
	public partial class PlayerInfo : IMessage {}

	[Message(HotfixOpcode.C2G_PlayerInfo)]
	public partial class C2G_PlayerInfo : IRequest {}

	[Message(HotfixOpcode.G2C_PlayerInfo)]
	public partial class G2C_PlayerInfo : IResponse {}

	[Message(HotfixOpcode.C2R_CreateRoom)]
	public partial class C2R_CreateRoom : IRequest {}

	[Message(HotfixOpcode.M2G_CreateRoom)]
	public partial class M2G_CreateRoom : IResponse {}

	[Message(HotfixOpcode.roominfo)]
	public partial class roominfo {}

	[Message(HotfixOpcode.C2R_EnterRoomSelect)]
	public partial class C2R_EnterRoomSelect : IRequest {}

	[Message(HotfixOpcode.R2C_EnterRoomSelect)]
	public partial class R2C_EnterRoomSelect : IResponse {}

	[Message(HotfixOpcode.C2R_EnterRoom)]
	public partial class C2R_EnterRoom : IRequest {}

	[Message(HotfixOpcode.R2C_EnterRoom)]
	public partial class R2C_EnterRoom : IResponse {}

	[Message(HotfixOpcode.C2G_EnterMapFinish)]
	public partial class C2G_EnterMapFinish : IMessage {}

	[Message(HotfixOpcode.G2C_Reconnect)]
	public partial class G2C_Reconnect : IMessage {}

	[Message(HotfixOpcode.C2G_LeaveGame)]
	public partial class C2G_LeaveGame : IRequest {}

	[Message(HotfixOpcode.G2C_LeaveGame)]
	public partial class G2C_LeaveGame : IResponse {}

}
namespace ETHotfix
{
	public static partial class HotfixOpcode
	{
		 public const ushort C2R_Login = 10001;
		 public const ushort R2C_Login = 10002;
		 public const ushort C2R_Register = 10003;
		 public const ushort R2C_Register = 10004;
		 public const ushort C2G_LoginGate = 10005;
		 public const ushort G2C_LoginGate = 10006;
		 public const ushort G2C_TestHotfixMessage = 10007;
		 public const ushort C2M_TestActorRequest = 10008;
		 public const ushort M2C_TestActorResponse = 10009;
		 public const ushort PlayerInfo = 10010;
		 public const ushort C2G_PlayerInfo = 10011;
		 public const ushort G2C_PlayerInfo = 10012;
		 public const ushort C2R_CreateRoom = 10013;
		 public const ushort M2G_CreateRoom = 10014;
		 public const ushort roominfo = 10015;
		 public const ushort C2R_EnterRoomSelect = 10016;
		 public const ushort R2C_EnterRoomSelect = 10017;
		 public const ushort C2R_EnterRoom = 10018;
		 public const ushort R2C_EnterRoom = 10019;
		 public const ushort C2G_EnterMapFinish = 10020;
		 public const ushort G2C_Reconnect = 10021;
		 public const ushort C2G_LeaveGame = 10022;
		 public const ushort G2C_LeaveGame = 10023;
	}
}
