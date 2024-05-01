import argparse
from dataclasses import dataclass
from functools import partial
import io
import socket
import sys

import numpy as np
from PIL import Image

import cv2
import pyvirtualcam


PORT = 5065


@dataclass
class Args:
    adress: str = "127.0.0.1"
    port: int = 5065

    virtual_cam: bool = False
    size: str = "512x512"


def parse_args() -> Args:
    parser = argparse.ArgumentParser()
    parser.add_argument("--adress", type=str, default="127.0.0.1")
    parser.add_argument("--port", type=int, default=5065)
    parser.add_argument(
        "--virtual_cam", action=argparse.BooleanOptionalAction, default=True
    )
    parser.add_argument("--size", type=str, default="512x512")
    args = parser.parse_args()
    print(args.virtual_cam)
    return Args(
        adress=args.adress,
        port=args.port,
        virtual_cam=args.virtual_cam,
        size=args.size,
    )


def parse_size(size: str) -> tuple[int, int]:
    w, h = [int(s) for s in size.split("x")]
    return w, h


def virtual_cam_loop(s: socket.socket, size: str):
    w, h = parse_size(size)
    with pyvirtualcam.Camera(w, h, 20) as cam:
        while True:
            frame = receive_frame(s)
            frame = cv2.cvtColor(np.array(frame), cv2.COLOR_RGBA2RGB)
            cam.send(frame)


def opencv_loop(s: socket.socket):
    while True:
        frame = receive_frame(s)
        frame = cv2.cvtColor(np.array(frame), cv2.COLOR_RGBA2BGR)
        cv2.imshow("yolo", frame)
        cv2.waitKey(1)


def receive_frame(s: socket.socket) -> np.ndarray:
    conn, _ = s.accept()
    with conn:
        received_data = bytearray()
        while True:
            data = conn.recv(1024)
            if not data:
                break
            received_data.extend(data)

    return np.array(Image.open(io.BytesIO(received_data)))


def start_server(args: Args):
    address = (args.adress, args.port)

    if args.virtual_cam:
        loop_fn = partial(virtual_cam_loop, size=args.size)
    else:
        loop_fn = opencv_loop

    try:
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            # AF_INET for communication of network
            # SOCK_STREAM for TCP socket
            s.bind(address)
            s.listen(1)
            loop_fn(s)

    except OSError as e:
        print(f"Error while connecting :: {str(e)}")
        sys.exit()


def main():
    start_server(args)


if __name__ == "__main__":
    args = parse_args()
    start_server(args)

    # main()
