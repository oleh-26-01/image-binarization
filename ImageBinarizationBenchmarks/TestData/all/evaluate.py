import os
import cv2
import numpy as np
from multiprocessing.pool import ThreadPool

os.chdir(os.path.dirname(__file__))

def evaluate_binarization(ground_truth_dir, binarized_dir, num_processes=4):
    """
    Evaluates a binarization algorithm using a thread pool.

    Args:
        ground_truth_dir: Path to the directory containing ground truth images.
        binarized_dir: Path to the directory containing binarized images.
        num_processes: Number of threads in the pool (default: 4).

    Returns:
        A dictionary containing the evaluation metrics.
    """

    # Get lists of image files in both directories
    ground_truth_files = sorted(os.listdir(ground_truth_dir))
    binarized_files = sorted(os.listdir(binarized_dir))

    # Assert that the number of images in both directories is the same
    assert len(ground_truth_files) == len(binarized_files), "Number of images in both directories must be equal."

    # Create a thread pool
    pool = ThreadPool(processes=num_processes)

    # Use the pool to process images in parallel
    results = pool.map(process_image_pair, [(os.path.join(ground_truth_dir, filename), os.path.join(binarized_dir, filename)) for filename in ground_truth_files])

    # Close the pool
    pool.close()
    pool.join()

    # Extract metrics from results
    perr_list = [result[0] for result in results]
    mse_list = [result[1] for result in results]
    snr_list = [result[2] for result in results]
    psnr_list = [result[3] for result in results]

    # Calculate average metrics and PERR variation
    perr = np.mean(perr_list)
    mse = np.mean(mse_list)
    snr = np.mean(snr_list)
    psnr = np.mean(psnr_list)
    perr_variation = np.std(perr_list)

    return {
        'perr': perr,
        'mse': mse,
        'snr': snr,
        'psnr': psnr,
        'perr_variation': perr_variation
    }

def process_image_pair(paths):
    """
    Processes a single pair of images to calculate metrics.

    Args:
        paths: A tuple containing the paths to the ground truth and binarized images.

    Returns:
        A tuple containing the PERR, MSE, SNR, and PSNR for the image pair.
    """

    ground_truth_path, binarized_path = paths
    ground_truth_img = cv2.imread(ground_truth_path, cv2.IMREAD_GRAYSCALE)
    binarized_img = cv2.imread(binarized_path, cv2.IMREAD_GRAYSCALE)
    perr, mse, snr, psnr = calculate_metrics(ground_truth_img, binarized_img)
    return perr, mse, snr, psnr

def calculate_metrics(ground_truth_img, binarized_img):
    """
    Core calculation of metrics, separated for better readability and potential future optimization.

    Args:
        ground_truth_img: Ground truth image (grayscale).
        binarized_img: Binarized image (grayscale).

    Returns:
        A tuple containing the calculated metrics:
            - perr: Pixel Error Rate
            - mse: Mean Squared Error
            - snr: Signal-to-Noise Ratio
            - psnr: Peak Signal-to-Noise Ratio
    """

    # Calculate PERR
    perr = np.sum(ground_truth_img != binarized_img) / (ground_truth_img.shape[0] * ground_truth_img.shape[1])

    # Calculate MSE
    mse = np.mean((ground_truth_img - binarized_img)**2)

    # Calculate SNR
    snr = 10 * np.log10(np.var(ground_truth_img) / mse)

    # Calculate PSNR
    psnr = 10 * np.log10(255**2 / mse)

    return perr, mse, snr, psnr


# Example usage:
ground_truth_dir = './GT/'  # Replace with the actual path
binarized_dir = './img/binarized/Sauvola/'  # Replace with the actual path

results = evaluate_binarization(ground_truth_dir, binarized_dir)

print("Evaluation Results:")
print("PERR:", results['perr'])
print("MSE:", results['mse'])
print("SNR:", results['snr'])
print("PSNR:", results['psnr'])
print("PERR Variation:", results['perr_variation'])