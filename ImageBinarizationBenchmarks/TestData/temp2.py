import matplotlib.pyplot as plt

# Extracting data for Otsu and Sauvola algorithms
algorithms = ['Otsu', 'Sauvola']
methods = ['Imperative', 'Declarative', 'Functional', 'OOP']

otsu_times = [1.097, 6.798, 1.172, 1.362]
sauvola_times = [23.140, 31.410, 23.910, 23.060]

# Plotting the data
# v1
# plt.figure(figsize=(10, 6))
# plt.bar([f'{m} {a}' for a in algorithms for m in methods], otsu_times + sauvola_times, color=['b', 'g', 'r', 'c'] * 2)
# plt.title("Mean Processing Time (ms) by Method and Algorithm")
# plt.ylabel("Mean Time (ms)")
# plt.xlabel("Method and Algorithm")
# plt.show()

# v2, split by algorithm into two subplots; labels vertical
# fig, axs = plt.subplots(1, 2, figsize=(10, 12))

# for i, algorithm in enumerate(algorithms):
#     ax = axs[i]
#     ax.bar(methods, otsu_times if algorithm == 'Otsu' else sauvola_times, color=['b', 'g', 'r', 'c'])
#     ax.set_title(f"Mean Processing Time (ms) by Method for {algorithm} Algorithm")
#     ax.set_ylabel("Mean Time (ms)")
#     ax.set_xlabel("Method")
#     ax.grid(axis='y')
    
# plt.tight_layout()
# plt.show()

# v3 as v2, but with log scale from min to max time

# increase font size
plt.rcParams.update({'font.size': 16})
fig, axs = plt.subplots(1, 2, figsize=(10, 16))

for i, algorithm in enumerate(algorithms):
    ax = axs[i]
    ax.bar(methods, otsu_times if algorithm == 'Otsu' else sauvola_times, color=['b', 'g', 'r', 'c'])
    ax.set_title(f"Mean Processing Time (ms) by Method for {algorithm} Algorithm")
    ax.set_ylabel("Mean Time (ms)")
    ax.set_xlabel("Method")
    ax.set_yscale('log')
    ax.grid(axis='y')
    # ax.set_ylim(min(otsu_times + sauvola_times), max(otsu_times + sauvola_times))
    
# plt.tight_layout()
plt.show()