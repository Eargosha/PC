import time
import numpy as np
import torch

# Размеры матриц
n = 400
m = 400
k = 400

# Генерация случайных матриц
A = np.random.rand(n, m)
B = np.random.rand(m, k)

# 1. Перемножение матриц как списков Python
A_list = A.tolist()
B_list = B.tolist()

start_time = time.time()
result_list = [[sum(a * b for a, b in zip(A_row, B_col)) for B_col in zip(*B_list)] for A_row in A_list]
python_time = time.time() - start_time
print(f"Python lists time: {python_time:.4f} seconds")

# 2. Перемножение матриц как NumPy arrays
start_time = time.time()
result_numpy = np.dot(A, B)
numpy_time = time.time() - start_time
print(f"NumPy time: {numpy_time:.4f} seconds")

# 3. Перемножение матриц как PyTorch tensors на CPU
A_torch_cpu = torch.tensor(A)
B_torch_cpu = torch.tensor(B)

start_time = time.time()
result_torch_cpu = torch.matmul(A_torch_cpu, B_torch_cpu)
torch_cpu_time = time.time() - start_time
print(f"PyTorch CPU time: {torch_cpu_time:.4f} seconds")

# 4. Перемножение матриц как PyTorch tensors на GPU (если доступен)

if torch.cuda.is_available():
    print(torch.cuda.get_device_name(0))  # Должно вывести название вашего GPU (например, "RTX 2070")
    A_torch_gpu = torch.tensor(A).cuda()
    B_torch_gpu = torch.tensor(B).cuda()

    start_time = time.time()
    result_torch_gpu = torch.matmul(A_torch_gpu, B_torch_gpu)
    torch_gpu_time = time.time() - start_time
    print(f"PyTorch GPU time: {torch_gpu_time:.4f} seconds")
else:
    print("GPU is not available")

# Повторение замеров для надёжности
num_repeats = 5
python_times = []
numpy_times = []
torch_cpu_times = []
torch_gpu_times = []

for _ in range(num_repeats):
    # Python lists
    start_time = time.time()
    result_list = [[sum(a * b for a, b in zip(A_row, B_col)) for B_col in zip(*B_list)] for A_row in A_list]
    python_times.append(time.time() - start_time)

    # NumPy
    start_time = time.time()
    result_numpy = np.dot(A, B)
    numpy_times.append(time.time() - start_time)

    # PyTorch CPU
    start_time = time.time()
    result_torch_cpu = torch.matmul(A_torch_cpu, B_torch_cpu)
    torch_cpu_times.append(time.time() - start_time)

    # PyTorch GPU
    if torch.cuda.is_available():
        start_time = time.time()
        result_torch_gpu = torch.matmul(A_torch_gpu, B_torch_gpu)
        torch_gpu_times.append(time.time() - start_time)

# Вывод среднего времени
print(f"Average Python lists time: {np.mean(python_times):.4f} seconds")
print(f"Average NumPy time: {np.mean(numpy_times):.4f} seconds")
print(f"Average PyTorch CPU time: {np.mean(torch_cpu_times):.4f} seconds")
if torch.cuda.is_available():
    print(f"Average PyTorch GPU time: {np.mean(torch_gpu_times):.4f} seconds")
