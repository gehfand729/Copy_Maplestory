using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STD
{
	public class iShortestPath
	{
		public static int I = 999;

		public struct NodeSP
		{
			public bool visit;
			public int value;
			public int[] path;
			public int pathNum;
		}

		int tileX, tileY, tileW, tileH;
		NodeSP[] node;
		int[] path;

		public void set(int tx, int ty, int tw, int th)
		{
			tileX = tx;
			tileY = ty;
			tileW = tw;
			tileH = th;

			int tileXY = tileX * tileY;
			if (node != null)
			{
				if (node.Length >= tileXY)
					return;
			}
			node = new NodeSP[tileXY];
			for (int i = 0; i < tileXY; i++)
				node[i].path = new int[100];
			path = new int[tileXY];
		}

		public iShortestPath()
		{
			tileX = 0;
			tileY = 0;
			tileW = 0;
			tileH = 0;
			node = null;
			path = null;

			ma_ = new MethodAlgorithm[3]
			{
				new MethodAlgorithm(dijstra),
				new MethodAlgorithm(astar),
				new MethodAlgorithm(etc),
			};
			ma = ma_[0];
		}

		public enum SpAlgorithm
		{ 
			Dijstra = 0,
			Astar,
			Etc,
		}

		public void set(SpAlgorithm index)
		{
			 ma = ma_[(int)index];
		}

		delegate int MethodAlgorithm(int[] value, int s, int e, int[] path);
		MethodAlgorithm[] ma_;
		MethodAlgorithm ma;

		public int run(int[] value, iPoint ps, iPoint pe, iPoint[] ppath)
		{
			// ps -> s
			int x = (int)ps.x; x /= tileW;
			int y = (int)ps.y; y /= tileH;
			int s = tileX * y + x;
			// pe -> e
			x = (int)pe.x; x /= tileW;
			y = (int)pe.y; y /= tileH;
			int e = tileX * y + x;

			int pathNum = ma(value, s, e, path);

#if true    // �ߺ��� ��� ����(���̴� �̵����� ���� ����)                                
			for (int i = 0; i < pathNum; i++)
			{
				x = path[i] % tileX;
				y = path[i] / tileX;

				if (i < pathNum - 2)
				{
					// i = pathNum - 3
					// i + 2 = pathNum - 1
					int nextX = path[i + 2] % tileX;
					int nextY = path[i + 2] / tileX;

					// ����ó�� �ϴ� ��(���̴� ��)
					if (x != nextX && y != nextY)
					{   // i / i+1 / i+2
						//i += 2;
						// i / i+1 / (i+2)
						i++;
						continue;
					}
					// �����ϴ� ��
					if (x == nextX || y == nextY)
					{
						// ���� �����ؾ� �Ұ��� ���̴� ��
						if( i + 1 < pathNum - 2 )
						{
							nextX = path[i + 3] % tileX;
							nextY = path[i + 3] / tileX;
							if (x != nextX && y != nextY)
								continue;
						}

						// [i + 1] remove
						pathNum--;
						for (int j = i + 1; j < pathNum; j++)
							path[j] = path[j + 1];

						i--;
						continue;
					}
				}
			}
#endif
			// path[i] => ppath[i]
			for (int i = 0; i < pathNum; i++)
			{
				x = path[i] % tileX;
				y = path[i] / tileX;

#if true// �ʹ� ���̴� �̵� ����
				if (i < pathNum - 2)
				{
					// i = pathNum - 3
					// i + 2 = pathNum - 1
					int nextX = path[i + 2] % tileX;
					int nextY = path[i + 2] / tileX;

					// ����ó�� �ϴ� ��(���̴� ��)
					if (x != nextX && y != nextY)
					{
						ppath[i].x = tileW * x + tileW / 2;
						ppath[i].y = tileH * y + tileH / 2;
						
						x = path[i + 2] % tileX;
						y = path[i + 2] / tileX;
						ppath[i + 2].x = tileW * x + tileW / 2;
						ppath[i + 2].y = tileH * y + tileH / 2;
						iPoint p = (ppath[i] + ppath[i + 2]) / 2;

						int ix = path[i + 1] % tileX;
						int iy = path[i + 1] / tileX;
						ppath[i + 1].x = tileW * ix + tileW / 2;
						ppath[i + 1].y = tileH * iy + tileH / 2;

						ppath[i + 1] = (ppath[i + 1] + p) / 2;

						i++;
						continue;
					}
				}
#endif
				ppath[i].x = tileW * x + tileW / 2;
				ppath[i].y = tileH * y + tileH / 2;
			}
			return pathNum;
		}

		private int dijstra(int[] value, int s, int e, int[] path)
		{
			// �ʱ�ȭ
			// - ��� ���� ���Ѱ����� ����
			// - ��������� ��� 0
			// - ��� ���� �湮���� �������� ����
			int n = value.Length;
			for (int i = 0; i < n; i++)
			{
				ref NodeSP p = ref node[i];
				p.visit = false;
				p.value = I;
#if false
				p.pathNum = 0;
#else
				p.path[0] = i;
				p.pathNum = 1;
#endif
			}
			node[s].value = 0;

			// �ݺ� ��� ���� �湮(1), 2)�� ������ �ݺ�)
			// - 1) ����� ���� ���� ���� ã�� �湮 ó��(���� ������, ������ 1������)
			// - 2) �湮�Ѱ����� ���� �մ� ���� ��� ����
			for (int i = 0; i < n; i++) 
			{
				// ��� ��� �湮�ϴµ� ����(�� 1���� �湮, ���� ��� ���ٸ� ����1������)
				int curr = -1;
				int min = I;
				for (int j = 0; j < n; j++)
				{
					if(node[j].visit==false && node[j].value < min)
					{
						curr = j;
						min = node[j].value;
					}
				}
				if (curr == -1)
					break;
				node[curr].visit = true;

				// �湮�� �ϸ�, ���� �ִ°��� ����� ���� (����������)
				for(int j = 0; j<4; j++)
				{
					int k;
					if( j==0 )
					{   // left
						if (curr % tileX == 0) continue;
						k = curr - 1;
					}
					else if( j==1 )
					{   // right
						if (curr % tileX == tileX - 1) continue;
						k = curr + 1;
					}
					else if( j==2 )
					{   // top
						if (curr / tileX == 0) continue;
						k = curr - tileX;
					}
					else// if( j==3)
					{   // bottom
						if (curr / tileX == tileY - 1) continue;
						k = curr + tileX;
					}

#if true
					ref NodeSP n0 = ref node[k];
					if (n0.visit) continue;
					ref NodeSP n1 = ref node[curr];
					int val = n1.value + value[k];
					if (n0.value > val)
					{
						n0.value = val;

						for(int m=0; m < n1.pathNum; m++)
							n0.path[m] = n1.path[m];
						n0.path[n1.pathNum] = k;
						n0.pathNum = n1.pathNum + 1;
					}
#else
					if (node[k].visit) continue;
					int val = node[curr].value + value[k];
					if (node[k].value > val)
					{
						node[k].value = val;

						for (int m = 0; m < node[curr].pathNum; m++)
							node[k].path[m] = node[curr].path[m];
						node[k].path[node[curr].pathNum] = k;
						node[k].pathNum = node[curr].pathNum + 1;
					}
#endif
				}
			}
			// ��� ����� ó�� �Ϸ�

			// ���
			// ������ �ϴ°��� �ּҺ��
			for (int i = 0; i < node[e].pathNum; i++)
				path[i] = node[e].path[i];
			return node[e].pathNum;
		}

		private int astar(int[] value, int s, int e, int[] path)
		{
			Debug.Log("astar");
			return dijstra(value, s, e, path);
		}

		private int etc(int[] value, int s, int e, int[] path)
		{
			Debug.Log("etcw");
			return dijstra(value, s, e, path);
		}
	}

}
