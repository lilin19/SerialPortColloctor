import tensorflow
print(hash)

def __call__(self, left_state, right_state, extra_input=None):
        with tf.variable_scope('TreeLSTM'):
            c1, h1 = left_state
            c2, h2 = right_state

            if extra_input is not None:
                input_concat = tf.concat((extra_input, h1, h2), axis=1)
            else:
                input_concat = tf.concat((h1, h2), axis=1)
            concat = tf.layers.dense(input_concat, 5 * self._num_cells)
            i, f1, f2, o, g = tf.split(concat, 5, axis=1)
            i = tf.sigmoid(i)
            f1 = tf.sigmoid(f1)
            f2 = tf.sigmoid(f2)
            o = tf.sigmoid(o)
            g = tf.tanh(g)

            cnew = f1 * c1 + f2 * c2 + i * g
            hnew = o * cnew

            newstate = LSTMStateTuple(c=cnew, h=hnew)
            return hnew, newstate 