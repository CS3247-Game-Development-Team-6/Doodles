import os
import random
import numpy as np
from PIL import Image
from matplotlib import pyplot as plt


# start_point = (0, 0)
# end_point = (5, 4)


def manhattan(a, b):
    return sum(abs(val1 - val2) for val1, val2 in zip(a, b))


w, h = 10, 10

start_point = (random.randint(0, w - 1), random.randint(0, h - 1))
end_point = (random.randint(0, w - 1), random.randint(0, h - 1))

while manhattan(start_point, end_point) < 15:
    start_point = (random.randint(0, w - 1), random.randint(0, h - 1))
    end_point = (random.randint(0, w - 1), random.randint(0, h - 1))

matrix = [[0 for _ in range(w)] for _ in range(h)]

def print_matrix(m):
    for r in range(len(m)):
        for c in range(len(m[0])):
            print(m[r][c], end=' ')
        print()


def gallery(array, ncols=10):
    nindex, height, width, intensity = array.shape
    nrows = nindex // ncols
    assert nindex == nrows * ncols
    # want result.shape = (height*nrows, width*ncols, intensity)
    result = (array.reshape(nrows, ncols, height, width, intensity)
              .swapaxes(1, 2)
              .reshape(height * nrows, width * ncols, intensity))
    return result


def change_img(array, x, y, img_tup):
    img_url, rotation = img_tup
    new_image = Image.open(img_url).convert('RGB').rotate(-rotation)
    array[int(str(x)+str(y))] = new_image
    return array


def make_array():
    return np.array([np.asarray(Image.open('images/sq.png').convert('RGB'))] * 100)


image_conversion = {
    'base': ('images/sq_base.png', 0),
    'spawn': ('images/sq_enemy.png', 0),
    'll': ('images/sq_str8.png', 180),
    'l': ('images/sq_str8.png', 180),
    'lu': ('images/sq_hook.png', 270),
    'ld': ('images/sq_hook.png', 0),
    'rr': ('images/sq_str8.png', 0),
    'r': ('images/sq_str8.png', 0),
    'ru': ('images/sq_hook.png', 180),
    'rd': ('images/sq_hook.png', 90),
    'dd': ('images/sq_str8.png', 90),
    'd': ('images/sq_str8.png', 90),
    'dl': ('images/sq_hook.png', 180),
    'dr': ('images/sq_hook.png', 270),
    'uu': ('images/sq_str8.png', 270),
    'u': ('images/sq_str8.png', 270),
    'ul': ('images/sq_hook.png', 90),
    'ur': ('images/sq_hook.png', 0),
}

delta_pos = {
    0: (-1, 0),  # left
    1: (0, -1),  # up
    2: (1, 0),  # right
    3: (0, 1)  # down
}

move_name = {
    0: "left",
    1: "up",
    2: "right",
    3: "down"
}


def score_path(matrix, x, y):
    score = 0
    for i in range(-1, 2):
        for j in range(-1, 2):
            try:
                if y + i >= 0 and x + j >= 0:
                    if matrix[y + i][x + j] == "O":
                        # matrix[y + i][x + j] = "B"     # for debugging
                        score += 1
                    else:
                        score -= 2
            except IndexError:
                pass
    return score


def find_move_to_move_str(this_move, last_move=None):
    str_move = ""
    if last_move == 0:
        str_move += 'l'
    elif last_move == 1:
        str_move += 'u'
    elif last_move == 2:
        str_move += 'r'
    elif last_move == 3:
        str_move += 'd'

    if this_move == 0:
        str_move += 'l'
    elif this_move == 1:
        str_move += 'u'
    elif this_move == 2:
        str_move += 'r'
    elif this_move == 3:
        str_move += 'd'

    return str_move


allowed_x_range = range(0, w)
allowed_y_range = range(0, h)

found_a_path = False
number_of_tries = 0
length_of_path = 0

while number_of_tries < 1000:
    ### RESET MATRIX ###
    array = make_array()  # creates the starting array
    change_img(array, start_point[1], start_point[0], image_conversion['spawn'])
    change_img(array, end_point[1], end_point[0], image_conversion['base'])

    # backend-end for move simulations
    position_x, position_y = end_point[0], end_point[1]
    matrix = [["O" for _ in range(w)] for _ in range(h)]
    matrix[start_point[1]][start_point[0]] = "B"
    matrix[end_point[1]][end_point[0]] = "S"

    length_of_path = 0
    found_a_path = False
    path_idx = 1
    score = 0

    current_coords = (start_point[1], start_point[0])
    last_move = None
    this_move = None

    for _ in range(100):  # 100 moves per iteration
        previous_coords = current_coords

        last_move = this_move

        action = random.randint(0, 3)
        new_x = position_x + delta_pos[action][0]
        new_y = position_y + delta_pos[action][1]
        if new_x in allowed_x_range and new_y in allowed_y_range:
            if matrix[new_y][new_x] == "B":
                found_a_path = True
                str_move = find_move_to_move_str(this_move, last_move)
                change_img(array, previous_coords[0], previous_coords[1], image_conversion[str_move])
                break

            if matrix[new_y][new_x] != "S" and not isinstance(matrix[new_y][new_x], int):
                score += score_path(matrix, new_x, new_y)
                position_x = new_x
                position_y = new_y
                matrix[position_y][position_x] = path_idx
                current_coords = (position_y, position_x)
                this_move = action
                path_idx += 1
                length_of_path += 1

                if length_of_path > 1:
                    str_move = find_move_to_move_str(this_move, last_move)
                    change_img(array, previous_coords[0], previous_coords[1], image_conversion[str_move])

    if found_a_path:
        man_dist = manhattan(start_point, end_point)
        if man_dist * 2 > length_of_path > man_dist * 1.5 and score > 50:
            print_matrix(matrix)
            print(f'\nScore: {score}')
            break

    number_of_tries += 1

fig = plt.figure(figsize=(5, 5))
result = gallery(array)
plt.imshow(result)
plt.show()
