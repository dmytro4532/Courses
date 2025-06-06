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
                    name: 'Current Image',
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

            enqueueSnackbar(`Course ${initialValues ? 'updated' : 'created'} successfully`, { variant: 'success', autoHideDuration: 3000 });
            form.resetFields();
            setFileList([]);
            onSuccess();
        } catch (error: any) {
            if (error.response?.data?.errors) {
                const validationErrors = error.response.data.errors;
                const errorMessages = validationErrors.map((err: any) => `${err.description}`).join('\n');
                enqueueSnackbar(errorMessages, { variant: 'error', autoHideDuration: 5000 });
            } else {
                enqueueSnackbar('Failed to save course', { variant: 'error', autoHideDuration: 3000 });
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
                enqueueSnackbar('You can only upload image files!', { variant: 'error', autoHideDuration: 3000 });
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
                label="Title"
                rules={[{ required: true, message: 'Please input the title!' }]}
            >
                <Input />
            </Form.Item>

            <Form.Item
                name="description"
                label="Description"
                rules={[{ required: true, message: 'Please input the description!' }]}
            >
                <Input.TextArea rows={4} />
            </Form.Item>

            <Form.Item label="Image">
                <Upload
                    {...uploadProps}
                    maxCount={1}
                    listType="picture-card"
                >
                    {fileList.length === 0 && (
                        <div>
                            <PlusOutlined />
                            <div style={{ marginTop: 8 }}>Upload</div>
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
                        {initialValues ? 'Update' : 'Create'}
                    </Button>
                    <Button onClick={onCancel}>
                        Cancel
                    </Button>
                </Space>
            </Form.Item>
        </Form>
    );
}; 