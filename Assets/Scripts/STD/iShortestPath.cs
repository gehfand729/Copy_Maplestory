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

#if true    // 중복된 경로 제거(꺽이는 이동지역 제거 제외)                                
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

					// 예외처리 하는 곳(꺽이는 곳)
					if (x != nextX && y != nextY)
					{   // i / i+1 / i+2
						//i += 2;
						// i / i+1 / (i+2)
						i++;
						continue;
					}
					// 제거하는 곳
					if (x == nextX || y == nextY)
					{
						// 만약 제거해야 할곳이 꺽이는 곳
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

#if true// 너무 꺽이는 이동 방지
				if (i < pathNum - 2)
				{
					// i = pathNum - 3
					// i + 2 = pathNum - 1
					int nextX = path[i + 2] % tileX;
					int nextY = path[i + 2] / tileX;

					// 예외처리 하는 곳(꺽이는 곳)
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
			// 초기화
			// - 모든 곳을 무한값으로 셋팅
			// - 출발지점만 비용 0
			// - 모든 곳을 방문하지 않음으로 셋팅
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

			// 반복 모든 곳을 방문(1), 2)의 과정을 반복)
			// - 1) 비용이 가장 작은 곳을 찾음 방문 처리(만약 같으면, 임의의 1개선택)
			// - 2) 방문한곳에서 갈수 잇는 곳의 비용 구함
			for (int i = 0; i < n; i++) 
			{
				// 모든 노드 방문하는데 목적(단 1개씩 방문, 만약 비용 같다면 임의1개선택)
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

				// 방문을 하면, 갈수 있는곳의 비용을 갱신 (작은값으로)
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
			// 모든 비용이 처리 완료

			// 결과
			// 가려고 하는곳의 최소비용
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
