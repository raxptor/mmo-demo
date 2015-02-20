using System;
using System.Collections.Generic;
using netki;

namespace demoserver
{
	class MainClass
	{
        public static Random r = new Random();

		public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            netki.Bitstream.Buffer buf = netki.Bitstream.Buffer.Make(new byte[1024]);
            Bitstream.PutCompressedUint(buf, 23);
            Bitstream.PutCompressedInt(buf, -4);
            Bitstream.PutCompressedInt(buf, 1032);
            Bitstream.PutCompressedInt(buf, -1024);
            Bitstream.PutCompressedInt(buf, 42);
            buf.Flip();
            uint x5 = Bitstream.ReadCompressedUint(buf);
            int x1 = Bitstream.ReadCompressedInt(buf); 
            int x2 = Bitstream.ReadCompressedInt(buf);
            int x3 = Bitstream.ReadCompressedInt(buf);
            int x4 = Bitstream.ReadCompressedInt(buf);
            Console.WriteLine("ah " + x1 + " " + x2 + " " + x3 + " " + x4 + " " + x5);
        }

        /*
        public static void InternetSimulate(List<Bitstream.Buffer> src, PacketLane lane)
        {
            if (src.Count == 0)
                return;

            int pick = 0;

            // packet arriving in order.
            if (r.NextDouble() > 0.95f)
                pick = r.Next() % src.Count;

            int bytepos = src[pick].bytepos;
            int bitpos = src[pick].bitpos;

            // packets delivered
            if (r.NextDouble() < 0.50f)
                lane.Incoming(src[pick]);

            src[pick].bitpos = bitpos;
            src[pick].bytepos = bytepos;

            // packet duplication
           
            if (r.NextDouble() < 0.90f)
               src.RemoveAt(pick);
        }

            return;
           
            PacketLane peer1 = new PacketLaneUnreliableOrdered();
            PacketLane peer2 = new PacketLaneUnreliableOrdered();

            uint[] sendme = new uint[1024];
            for (int i = 0; i < sendme.Length; i++)
                sendme[i] = (uint)(1000 + i);//(uint)(0x3411 * i | i);
                
            int sendPos1 = 0, sendPos2 = 0;

            List<Bitstream.Buffer> toPeer1 = new List<Bitstream.Buffer>();
            List<Bitstream.Buffer> toPeer2 = new List<Bitstream.Buffer>();
            List<Bitstream.Buffer> recv1 = new List<Bitstream.Buffer>();
            List<Bitstream.Buffer> recv2 = new List<Bitstream.Buffer>();

            Random r = new Random();

            int timeout = 0;
            while (true)
            {
                System.Threading.Thread.Sleep(0);
                if (r.NextDouble() < 0.02f && sendPos1 < sendme.Length)
                {
                    Console.WriteLine("Sending [" + sendPos1 + "] peer1 -> peer2");
                    Bitstream.Buffer buf = Bitstream.Buffer.Make(new byte[4]);
                    Bitstream.PutBits(buf, 32, sendme[sendPos1]);
                    buf.Flip();
                    sendPos1++;
                    peer1.Send(buf);
                }
                if (r.NextDouble() < 0.02f && sendPos2 < sendme.Length)
                {
                    Console.WriteLine("Sending [" + sendPos2 + "] peer2 -> peer1");
                    Bitstream.Buffer buf = Bitstream.Buffer.Make(new byte[4]);
                    Bitstream.PutBits(buf, 32, sendme[sendPos2]);
                    sendPos2++;
                    buf.Flip();
                    peer2.Send(buf);
                }

                while (true)
                {
                    Bitstream.Buffer tmp = peer1.Update(0.010f, delegate(Bitstream.Buffer send)
                        {
                            send.Flip();
                            toPeer2.Add(send);
                        });
                    if (tmp == null)
                        break;

                    uint rd = (uint)Bitstream.ReadBits(tmp, 32);
                    if (rd != sendme[recv1.Count])
                        Console.WriteLine("GOT WRONG DATA expected " + sendme[recv1.Count] + " got " + rd);
                    recv1.Add(tmp);
                }

                while (true)
                {
                    Bitstream.Buffer tmp = peer2.Update(0.010f, delegate(Bitstream.Buffer send)
                        {
                            send.Flip();
                            toPeer1.Add(send);
                        });
                    if (tmp == null)
                        break;
                    uint rd = (uint)Bitstream.ReadBits(tmp, 32);
                    if (recv2.Count >= sendme.Length)
                        Console.WriteLine("GOT BONUS " + rd);
                    if (rd != sendme[recv2.Count])
                        Console.WriteLine("GOT WRONG DATA expected " + sendme[recv2.Count] + " got " + rd);
                    recv2.Add(tmp);
                }

                if (recv1.Count == sendme.Length && recv2.Count == sendme.Length)
                {
                    Console.WriteLine("Done!");
                    break;
                }
                else
                {
                    Console.WriteLine("Peer1: OutQ:" + toPeer2.Count + " Recv:" + recv1.Count + " Peer2: OutQ:" + toPeer1.Count + " Recv:" + recv2.Count);
                }

                InternetSimulate(toPeer1, peer1);
                InternetSimulate(toPeer2, peer2);

                if (sendPos1 == sendme.Length && sendPos2 == sendme.Length)
                {
                    if (timeout++ > 100)
                        break;
                }
            }

            Console.WriteLine("PEER1 PL=" + peer1.ComputePacketLoss() + "  PEER2 PL=" + peer2.ComputePacketLoss());
        }
        */
    }
}
