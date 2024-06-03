import os
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker

script_dir = os.path.dirname(os.path.realpath(__file__))

db_path = os.path.join(script_dir, 'face_recognition.db')

engine = create_engine('sqlite:///' + db_path)
Session = sessionmaker(bind=engine)