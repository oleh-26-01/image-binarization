import pandas as pd
import matplotlib.pyplot as plt

# Parse the Data
data_string = """
| Method                 | CurrentImage | Mean      | Error     | StdDev    | Gen0      | Gen1      | Gen2      | Allocated   |
| TestImperativeOtsu     | Image1       |  4.065 ms | 0.0233 ms | 0.0194 ms |         - |         - |         - |      5.1 KB |
| TestDeclarativeOtsu    | Image1       | 21.079 ms | 0.1148 ms | 0.1074 ms |  992.1875 |  679.6875 |  117.1875 |  8962.24 KB |
| TestFunctionalOtsu     | Image1       |  2.790 ms | 0.0214 ms | 0.0189 ms |  187.5000 |  125.0000 |  125.0000 |  2847.66 KB |
| TestOOPOtsu            | Image1       |  3.305 ms | 0.0270 ms | 0.0240 ms |         - |         - |         - |      5.1 KB |
| TestImperativeSauvola  | Image1       | 45.050 ms | 0.2474 ms | 0.2314 ms |  406.2500 |  406.2500 |  406.2500 | 36936.28 KB |
| TestDeclarativeSauvola | Image1       | 65.369 ms | 0.7234 ms | 0.6767 ms | 1015.6250 | 1000.0000 | 1000.0000 | 43342.24 KB |
| TestFunctionalSauvola  | Image1       | 43.456 ms | 0.2212 ms | 0.2069 ms |  406.2500 |  406.2500 |  406.2500 | 55404.28 KB |
| TestOOPSauvola         | Image1       | 43.953 ms | 0.2079 ms | 0.1945 ms |  406.2500 |  406.2500 |  406.2500 | 36936.29 KB |
| TestImperativeOtsu     | Image2       |  4.043 ms | 0.0422 ms | 0.0352 ms |         - |         - |         - |      5.1 KB |
| TestDeclarativeOtsu    | Image2       | 20.853 ms | 0.1001 ms | 0.0936 ms |  992.1875 |  679.6875 |  117.1875 |  8962.26 KB |
| TestFunctionalOtsu     | Image2       |  2.809 ms | 0.0520 ms | 0.0578 ms |  187.5000 |  125.0000 |  125.0000 |  2847.66 KB |
| TestOOPOtsu            | Image2       |  3.308 ms | 0.0197 ms | 0.0175 ms |         - |         - |         - |      5.1 KB |
| TestImperativeSauvola  | Image2       | 45.163 ms | 0.2979 ms | 0.2641 ms |  406.2500 |  406.2500 |  406.2500 | 36936.28 KB |
| TestDeclarativeSauvola | Image2       | 63.572 ms | 0.3053 ms | 0.2856 ms | 1015.6250 | 1000.0000 | 1000.0000 | 43342.24 KB |
| TestFunctionalSauvola  | Image2       | 43.309 ms | 0.2537 ms | 0.2373 ms |  406.2500 |  406.2500 |  406.2500 | 55404.28 KB |
| TestOOPSauvola         | Image2       | 44.961 ms | 0.1875 ms | 0.1662 ms |  406.2500 |  406.2500 |  406.2500 | 36936.29 KB |
| TestImperativeOtsu     | Image3       |  4.023 ms | 0.0452 ms | 0.0378 ms |         - |         - |         - |      5.1 KB |
| TestDeclarativeOtsu    | Image3       | 20.729 ms | 0.1402 ms | 0.1312 ms |  992.1875 |  679.6875 |  117.1875 |  8962.24 KB |
| TestFunctionalOtsu     | Image3       |  2.796 ms | 0.0172 ms | 0.0161 ms |  187.5000 |  125.0000 |  125.0000 |  2847.66 KB |
| TestOOPOtsu            | Image3       |  3.306 ms | 0.0211 ms | 0.0187 ms |         - |         - |         - |      5.1 KB |
| TestImperativeSauvola  | Image3       | 44.517 ms | 0.2559 ms | 0.2394 ms |  406.2500 |  406.2500 |  406.2500 | 36936.28 KB |
| TestDeclarativeSauvola | Image3       | 63.651 ms | 0.2496 ms | 0.2335 ms | 1015.6250 | 1000.0000 | 1000.0000 | 43342.24 KB |
| TestFunctionalSauvola  | Image3       | 44.638 ms | 0.1890 ms | 0.1675 ms |  406.2500 |  406.2500 |  406.2500 | 55404.28 KB |
| TestOOPSauvola         | Image3       | 45.047 ms | 0.3043 ms | 0.2697 ms |  406.2500 |  406.2500 |  406.2500 | 36936.29 KB |"""

# Create a list of lists from the table data
data_rows = [line.split("|")[1:-1] for line in data_string.splitlines() if line.strip()]
data_rows = [row for row in data_rows if any(row)]

# Create a dataframe (modified)
columns = ["Method", "CurrentImage", "Mean", "Error", "StdDev", "Gen0", "Gen1", "Gen2", "Allocated"]
df = pd.DataFrame(data_rows[1:], columns=columns).applymap(lambda x: x.strip())

# remove memory columns
df = df.drop(columns=["Gen0", "Gen1", "Gen2", "Allocated"])

# Data Cleaning and Conversion (expanded)
for col in ["Mean", "Error", "StdDev"]:
    df[col] = df[col].str.replace(" ms", "").astype(float)

# Reshape Data
mean_df = df.pivot(index='Method', columns='CurrentImage', values='Mean')
error_df = df.pivot(index='Method', columns='CurrentImage', values='Error')
std_dev_df = df.pivot(index='Method', columns='CurrentImage', values='StdDev')

# Plotting with Expanded Time Analysis
plt.figure(figsize=(15, 8))

# Mean Time with Error Bars
plt.subplot(2, 1, 1)
mean_df.plot(kind='bar', yerr=error_df, capsize=4, ax=plt.gca())
plt.title("Mean Processing Time (ms) by Method and Image (with Error Bars)")
plt.ylabel("Mean Time (ms)")
plt.xlabel("Method")
plt.grid(axis='y')

# Mean Time with Std Deviation as Shaded Area
plt.subplot(2, 1, 2)
for image in mean_df.columns:
    plt.plot(mean_df.index, mean_df[image], label=image, marker='o')
    plt.fill_between(
        mean_df.index, 
        mean_df[image] - std_dev_df[image], 
        mean_df[image] + std_dev_df[image], 
        alpha=0.2
    )

plt.title("Mean Processing Time (ms) by Method and Image (with Std Deviation)")
plt.ylabel("Mean Time (ms)")
plt.xlabel("Method")
plt.legend(title="Image")
plt.grid(axis='y')

plt.tight_layout()
plt.show()