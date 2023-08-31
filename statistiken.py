import matplotlib.pyplot as plt
import numpy as np

x = [0,1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000]
#TTT AIvsAI
# loses = [4357,2594,2231,1562,1576,1396,1108,684,507,441,651]
# wins = [4388,6408,6655,7638,7225,7424,8089,8291,8760,8767,8334]
# draws = [1255, 998, 1114, 800, 1199, 1180, 803, 1025, 733,792, 1015]

#TTT AIvsRandom
# loses = [4434, 2787, 1855, 1326, 1015, 1118, 715, 681, 521, 256, 417]
# wins = [4242, 6202, 7158, 7787, 8191, 7855, 8278, 8429, 8674, 8975, 8814]
# draws = [1324, 1011, 987, 887, 974, 1027, 1007, 890, 805, 769, 769]

#TTT AIvsAI ohne LearnPhase
# loses = [4356, 3204, 2495, 1802, 1874, 1535, 1314, 1092, 900, 547, 725]
# wins = [4379, 5716, 6802, 7440, 7118, 7869, 7926, 8255, 8540, 8759, 8688]
# draws = [1265, 1080, 703, 758, 1008, 596, 760, 653, 560, 694, 587]

#TTT AIvsRandom ohne LearnPhase
# loses = [4369, 3188, 2431, 2180, 1387, 1424, 1440, 1055, 917, 501, 584]
# wins = [4321, 5898, 6483, 6775, 7779, 7683, 7805, 8208, 8465, 8991, 8865]
# draws = [1310, 914, 1086, 1045, 834, 893, 755, 737, 618, 508, 551]


# x = [0, 50000, 100000, 150000, 200000, 250000, 300000, 350000, 400000, 450000, 500000]
#4G AIvsAI
# loses = [4366, 2720, 2185, 2057, 2175, 1562, 1878, 1905, 1756, 1709, 1518]
# wins = [4380, 7269, 7807, 7860, 7810, 8433, 8116, 8086, 8229, 8271, 8473]
# draws = [1254, 11, 8, 11, 15, 5, 6, 9, 15, 20, 9]

#4G AIvsRandom
# loses = [4995, 2249, 1632, 1548, 1297, 1246, 1042, 1091, 881, 1016, 1003]
# wins = [4983, 7740, 8363, 8441, 8699, 8746, 8950, 8896, 9113,8980, 8996]
# draws = [22, 11, 5, 11, 4, 8, 8, 13, 6, 4, 1]

#4G AIvsAI ohne LearnPhase
# loses = [4351, 3068, 2281, 2098, 1960, 1833, 1498, 1280, 1469, 1366, 1199]
# wins = [4403, 6914, 7702, 7892, 8027, 8157, 8493, 8713, 8526, 8629, 8796]
# draws = [1246, 18, 17, 10, 13, 10, 9, 7, 5, 5, 5]

#TTT AIvsRandom ohne LearnPhase
# loses = [5007, 2858, 2325, 2087, 1548, 1733, 1951, 1376, 1361, 1266, 1312]
# wins = [4964, 7127, 7662, 7901, 8446, 8254, 8041, 8619, 8633, 8729, 8682]
# draws = [29, 15, 13, 12, 6, 13, 8, 5, 6, 5, 6]


# plot lines
# plt.plot(x, loses, label = "Niederlagen")
# plt.plot(x, wins, label = "Siege")
# plt.plot(x, draws, label = "Unentschieden")
# plt.grid(axis='y', color='0.5')
# plt.title("Training: AI vs. Random (ohne LearnPhase)")
# plt.legend()
# plt.show()

# RAM-Usage and #Unqiue states
x = [0,1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000]
# x = [0, 50000, 100000, 150000, 200000, 250000, 300000, 350000, 400000, 450000, 500000]

ram = [60, 61, 63, 65, 66, 67, 67, 68, 69, 69, 70]
states = [0, 1501, 2266, 2766, 3199, 3471, 3673, 3795, 3937, 3993, 4059]
# ram = [60, 331, 572, 1176, 1038, 1214, 1459, 1694, 1793, 1973, 2133]
# states = [0, 321490, 603363, 883459, 1148532, 1394671, 1661410, 1909633, 2159744, 2391009, 2621040]

fig, ax1 = plt.subplots()
ax1.set_ylabel('RAM-Auslastung in MB')
ax1.plot(x, ram)
ax2 = ax1.twinx()  # instantiate a second axes that shares the same x-axis
ax2.set_ylabel('Anzahl gespeicherter Zustände in Millionen')  # we already handled the x-label with ax1
ax2.plot(x, states)
fig.tight_layout()  # otherwise the right y-label is slightly clipped
plt.title("RAM-Auslastung und Anzahl gespeicherter Zustände nach x Partien")
plt.show()