import tqdm
import numpy as np
import time

from datetime import datetime
from collections.abc import Buffer


def start():
	print(f"Arrancando Python ({datetime.now().time()})")
	
def demo(size: int) -> Buffer:
	np.random.seed(42)
	a = np.random.randn(size, size)
	b = np.random.randn(size, size)

	start_time = time.time()
	
	print("Realizando la multiplicacion de matrices con NumPy...")
	# result = np.matmul(a, b)
	result = np.zeros((size, size))
	for i in tqdm.tqdm(range(size)):
		result[i, :] = np.dot(a[i, :], b)

	elapsed_time = time.time() - start_time
	print(f"Calculo completado en {elapsed_time:.2f} segundos")

	print("Primeros valores del resultado en Python:")
	print(result[:5, :5])
	print()

	return result

def stop():
	print(f"Fin de Python ({datetime.now().time()})")

if __name__ == "__main__":
	start()
	demo(size = 5000)
	stop()
