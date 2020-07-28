from multiprocessing import Process
import tailer

class FileTail:
    """A simple example class"""
    def __init__(self, log_fpath):
        open(log_fpath, 'a').close()
        p = Process(target=self.f, args=(log_fpath,))
        self.process = p
        p.start()

    def f(self, log_fpath):
        for line in tailer.follow(open(log_fpath)):
            print(line)

    def stop(self):
        self.process.terminate()
