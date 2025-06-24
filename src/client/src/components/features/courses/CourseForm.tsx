import { PlusOutlined } from '@ant-design/icons';
import { Form, Input, Upload, Button, Space } from 'antd';
import { enqueueSnackbar } from 'notistack';
import type { UploadFile } from 'antd/es/upload/interface';
import { useState } from 'react';
import type { CourseResponse } from '../../../types';
import api from '../../../api/axios';

interface CourseFormProps {
    initialValues?: CourseResponse;
    onSuccess: () => void;
    onCancel: () => void;
    loading?: boolean;
}

interface CourseFormData {
    title: string;
    description: string;
}

export const CourseForm = ({ initialValues, onSuccess, onCancel, loading }: CourseFormProps) => {
    const [form] = Form.useForm();
    const [fileList, setFileList] = useState<UploadFile[]>(
        initialValues?.imageUrl
            ? [
                {
                    uid: '-1',
                    name: 'Поточне зображення',
                    status: 'done',
                    url: initialValues.imageUrl,
                },
            ]
            : []
    );
    const [submitting, setSubmitting] = useState(false);

    const handleSubmit = async (values: CourseFormData) => {
        setSubmitting(true);
        try {
            const formData = new FormData();
            formData.append('title', values.title);
            formData.append('description', values.description);

            let response;
            if (initialValues) {
                formData.append('id', initialValues.id);
                response = await api.put(`/api/courses/${initialValues.id}`, formData);

                const imageFormData = new FormData();
                if (fileList[0]?.originFileObj) {
                    imageFormData.append('image', fileList[0].originFileObj as File);
                }
                imageFormData.append('courseId', initialValues.id);
                await api.post(`/api/courses/${initialValues.id}/image`, imageFormData);
            } else {
                response = await api.post('/api/courses', formData);

                if (fileList[0]?.originFileObj && response.data?.id) {
                    const imageFormData = new FormData();
                    imageFormData.append('courseId', response.data?.id);
                    imageFormData.append('image', fileList[0].originFileObj as File);
                    await api.post(`/api/courses/${response.data.id}/image`, imageFormData);
                }
            }

            enqueueSnackbar(`Курс ${initialValues ? 'оновлено' : 'створено'} успішно`, { variant: 'success', autoHideDuration: 3000 });
            form.resetFields();
            setFileList([]);
            onSuccess();
        } catch (error: any) {
            if (error.response?.data?.errors) {
                const validationErrors = error.response.data.errors;
                const errorMessages = validationErrors.map((err: any) => `${err.description}`).join('\n');
                enqueueSnackbar(errorMessages, { variant: 'error', autoHideDuration: 5000 });
            } else {
                enqueueSnackbar('Не вдалося зберегти курс', { variant: 'error', autoHideDuration: 3000 });
            }
            console.error('Error saving course:', error);
        } finally {
            setSubmitting(false);
        }
    };

    const uploadProps = {
        beforeUpload: (file: File) => {
            const isImage = file.type.startsWith('image/');
            if (!isImage) {
                enqueueSnackbar('Ви можете завантажувати лише файли зображень!', { variant: 'error', autoHideDuration: 3000 });
                return false;
            }
            return false;
        },
        onChange: ({ fileList }: { fileList: UploadFile[] }) => {
            setFileList(fileList);
        },
        fileList,
    };

    return (
        <Form
            form={form}
            layout="vertical"
            onFinish={handleSubmit}
            initialValues={initialValues}
        >
            <Form.Item
                name="title"
                label="Назва"
                rules={[{ required: true, message: 'Будь ласка, введіть назву!' }]}
            >
                <Input />
            </Form.Item>

            <Form.Item
                name="description"
                label="Опис"
                rules={[{ required: true, message: 'Будь ласка, введіть опис!' }]}
            >
                <Input.TextArea rows={4} />
            </Form.Item>

            <Form.Item label="Зображення">
                <Upload
                    {...uploadProps}
                    maxCount={1}
                    listType="picture-card"
                >
                    {fileList.length === 0 && (
                        <div>
                            <PlusOutlined />
                            <div style={{ marginTop: 8 }}>Завантажити</div>
                        </div>
                    )}
                </Upload>
            </Form.Item>

            <Form.Item>
                <Space>
                    <Button
                        type="primary"
                        htmlType="submit"
                        loading={submitting || loading}
                    >
                        {initialValues ? 'Оновити' : 'Створити'}
                    </Button>
                    <Button onClick={onCancel}>
                        Скасувати
                    </Button>
                </Space>
            </Form.Item>
        </Form>
    );
}; 