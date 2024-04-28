import io
import socket
import sys

from PIL import Image as I
from PIL.Image import Image
import numpy as np

import cv2
import pyvirtualcam

PORT = 5065


def init_TCP():
    address = ("127.0.0.1", PORT)
    try:
        # AF_INET for communication of network
        # SOCK_STREAM for TCP socket
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        s.bind(address)
        s.listen(1)

        with pyvirtualcam.Camera(512, 512, 20) as cam:
            while True:
                conn, addr = s.accept()
                with conn:
                    # received_data = []
                    received_data = bytearray()
                    while True:
                        data = conn.recv(1024)
                        if not data:
                            break
                        received_data.extend(data)

                frame = np.array(I.open(io.BytesIO(received_data)))

                # frame = cv2.cvtColor(np.array(frame), cv2.COLOR_RGBA2RGB)
                # cam.send(frame)

                frame = cv2.cvtColor(frame, cv2.COLOR_RGBA2BGR)
                cv2.imshow("yolo", frame)
                cv2.waitKey(1)

                # img.show()

    except OSError as e:
        print(f"Error while connecting :: {str(e)}")
        sys.exit()


def main():
    init_TCP()
    # a = bytearray()
    # print(len(a))


if __name__ == "__main__":
    main()
