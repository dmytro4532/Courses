import { LockOutlined, UserOutlined, MailOutlined } from '@ant-design/icons';
import { Alert, Button, Card, Form, Input, Typography, Space } from 'antd';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import type { AppDispatch, RootState } from '../../store';
import { registerAdmin } from '../../store/slices/authSlice';

const { Title, Text } = Typography;

const AdminUsers = () => {
  const [form] = Form.useForm();
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { status, registerError: error } = useSelector((state: RootState) => state.auth);

  const onFinish = async (values: { username: string; email: string; password: string }) => {
    dispatch(registerAdmin(values))
      .unwrap()
      .then(() => navigate('/admin/users'));
  };

  return (
    <div style={{ maxWidth: 400, margin: '24px auto', padding: '0 24px' }}>
      <Card>
        <Space direction="vertical" size="large" style={{ width: '100%' }}>
          <Title level={2} style={{ textAlign: 'center', marginBottom: 0 }}>
            Створити обліковий запис адміністратора
          </Title>
          <Text type="secondary" style={{ textAlign: 'center', display: 'block' }}>
            Створити новий обліковий запис адміністратора
          </Text>

          {error && (
            <Alert
              type="error"
              message={error}
              style={{ marginBottom: 0 }}
              showIcon
              closable
            />
          )}

          <Form
            form={form}
            name="register-admin"
            onFinish={onFinish}
            layout="vertical"
            requiredMark={false}
          >
            <Form.Item
              name="username"
              rules={[
                { required: true, message: 'Будь ласка, введіть ім\'я користувача' },
                { min: 3, message: 'Ім\'я користувача повинно містити щонайменше 3 символи' }
              ]}
            >
              <Input
                prefix={<UserOutlined />}
                placeholder="Ім'я користувача"
                size="large"
                disabled={status === 'loading'}
              />
            </Form.Item>

            <Form.Item
              name="email"
              rules={[
                { required: true, message: 'Будь ласка, введіть електронну адресу' },
                { type: 'email', message: 'Будь ласка, введіть дійсну електронну адресу' }
              ]}
            >
              <Input
                prefix={<MailOutlined />}
                placeholder="Електронна пошта"
                size="large"
                disabled={status === 'loading'}
              />
            </Form.Item>

            <Form.Item
              name="password"
              rules={[
                { required: true, message: 'Будь ласка, введіть пароль' },
                { min: 6, message: 'Пароль повинен містити щонайменше 6 символів' }
              ]}
            >
              <Input.Password
                prefix={<LockOutlined />}
                placeholder="Пароль"
                size="large"
                disabled={status === 'loading'}
              />
            </Form.Item>

            <Form.Item
              name="confirmPassword"
              dependencies={['password']}
              rules={[
                { required: true, message: 'Будь ласка, підтвердіть пароль' },
                ({ getFieldValue }) => ({
                  validator(_, value) {
                    if (!value || getFieldValue('password') === value) {
                      return Promise.resolve();
                    }
                    return Promise.reject(new Error('Два паролі не збігаються'));
                  },
                }),
              ]}
            >
              <Input.Password
                prefix={<LockOutlined />}
                placeholder="Підтвердіть пароль"
                size="large"
                disabled={status === 'loading'}
              />
            </Form.Item>

            <Form.Item>
              <Button
                type="primary"
                htmlType="submit"
                size="large"
                block
                loading={status === 'loading'}
              >
                {status === 'loading' ? 'Створення...' : 'Створити'}
              </Button>
            </Form.Item>
          </Form>
        </Space>
      </Card>
    </div>
  );
};

export default AdminUsers; 