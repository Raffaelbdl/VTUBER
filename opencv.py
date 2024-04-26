import io
import socket
import sys

from PIL import Image as I
from PIL.Image import Image
import numpy as np

PORT = 5068


def init_TCP():
    address = ("127.0.0.1", PORT)
    try:
        # AF_INET for communication of network
        # SOCK_STREAM for TCP socket
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        s.connect(address)
        print(
            "Connected to address:",
            socket.gethostbyname(socket.gethostname()) + ":" + str(PORT),
        )
        return s
    except OSError as e:
        print(f"Error while connecting :: {str(e)}")
        sys.exit()


def send_info_to_unity(s: socket.SocketType, info):
    msg = ""
    for i in info:
        msg += i
    try:
        s.send(bytes(msg, "utf-8"))
    except socket.error as e:
        print(f"Error while sending :: {str(e)}")
        sys.exit()


def send_image_to_unity(s: socket.SocketType, path: str):
    with open(path, "rb") as fd:
        buf = fd.read(1024)
        while buf:
            s.send(buf)
            buf = fd.read(1024)

    # data = open(path, "rb")
    # print(len(data))

    # try:
    #     s.send(data)
    # except socket.error as e:
    #     print(f"Error while sending image")
    #     sys.exit()


def load_png(path: str) -> Image:
    return I.open(path)


def main():
    s = init_TCP()
    # image = load_png("test.png")
    send_image_to_unity(s, "test.png")


if __name__ == "__main__":
    main()
