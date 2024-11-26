import torch
import time
from datetime import datetime
from collections.abc import Buffer


def start():
	print(f"Arrancando Python ({datetime.now().time()})")
	
def pytorch_demo() -> Buffer:
	device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
	print(f"Usando dispositivo: {device}")

	size = 12000
	a = torch.randn((size, size), device=device)
	b = torch.randn((size, size), device=device)

	start_time = time.time()
	
	print("Realizando la multiplicacion de matrices con PyTorch...")
	result = torch.matmul(a, b)

	# make sure calculus is completed when using GPU
	if device == "cuda":
		torch.cuda.synchronize()

	elapsed_time = time.time() - start_time
	print(f"Calculo completado en {elapsed_time:.2f} segundos")

	print("Primeros valores del resultado en Python:")
	print(result[:5, :5].cpu())
	print()

	return result.cpu().numpy()

def stop():
	print(f"Fin de Python ({datetime.now().time()})")

if __name__ == "__main__":
	start()
	pytorch_demo()
	stop()
